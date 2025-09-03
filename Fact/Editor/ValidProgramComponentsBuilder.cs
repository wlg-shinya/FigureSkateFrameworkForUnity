#if UNITY_EDITOR && WLG_DEVELOP
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact.Editor
{
    public class ValidProgramComponentsBuilder
    {
        public ValidProgramComponentsBuilder(string outputPath)
        {
            _outputPath = outputPath;

            // Addressablesのエントリ全取得
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(false);
            _addressableAssetEntry = new List<AddressableAssetEntry>();
            setting.GetAllAssets(_addressableAssetEntry, false);

            // フィギュアスケートデータの読み込み
            _eventObjects = LoadAssets<EventObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Event");
            _elementPlaceableSetObjects = LoadAssets<ElementPlaceableSetObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/ElementPlaceableSet");
            _elementPlaceableObjects = LoadAssets<ElementPlaceableObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/ElementPlaceable");
            _elementObjects = LoadAssets<ElementObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Element");
            _elementPlaceableSetArray = _elementPlaceableSetObjects.Select(x => x.data).ToArray();
            _elementPlaceableArray = _elementPlaceableObjects.Select(x => x.data).ToArray();
            foreach (var season in FactConstant.SEASONS)
            {
                var programObjects = LoadAssets<ProgramObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/Program");
                var programComponentRegulationObjects = LoadAssets<ProgramComponentRegulationObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/ProgramComponentRegulation");
                var elementBaseValueObjects = LoadAssets<ElementBaseValueObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/ElementBaseValue");
                _programObjectsMap.Add(season, programObjects.ToDictionary(x => x.data.id, x => x));
                _programComponentRegulationObjectsMap.Add(season, programComponentRegulationObjects.ToDictionary(x => x.data.id, x => x));
                _programComponentRegulationArray.Add(season, programComponentRegulationObjects.Select(x => x.data).ToArray());
                _elementBaseValueObjectsMap.Add(season, elementBaseValueObjects.ToDictionary(x => x.data.id, x => x));
                _elementBaseValueArray.Add(season, elementBaseValueObjects.Select(x => x.data).ToArray());

                // プログラム情報に配置可能な条件をセットアップして差し替え
                foreach (var eventObject in _eventObjects)
                {
                    var setupedProgramObjects = ProgramObjectQuery
                        .ByIds(_programObjectsMap[season].Values.ToList(), eventObject.data.programIds)
                        .Select(x => ProgramObjectQuery.SetupConditions(x, _programComponentRegulationObjectsMap[season].Values.ToList(), _elementPlaceableSetObjects))
                        .ToList();
                    foreach (var setupedProgramObject in setupedProgramObjects)
                    {
                        _programObjectsMap[season][setupedProgramObject.data.id] = setupedProgramObject;
                    }
                }

                // 構成に利用可能な全要素を事前に列挙
                _allPossibleElementIds.Add(season, _elementObjects
                    .Select(x => x.data.id)
                    .Where(id => _elementBaseValueObjectsMap[season].ContainsKey(id))
                    .ToList()
                    );
            }
        }

        // フルビルド
        public async Task FullBuild()
        {
            var maxConcurrency = Environment.ProcessorCount;
            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

            var allPrograms = FactConstant.SEASONS
                .SelectMany(season => _programObjectsMap[season].Values.Select(program => (season, program.data.id)))
                .ToList();

            // 全シーズンの全プログラム構成に対して構成可能な要素の組み割合わせを出力する
            var tasks = new List<Task>();
            foreach (var (season, programId) in allPrograms)
            {
                await semaphore.WaitAsync();
                var task = BuildOneProgram(season, programId).ContinueWith(t =>
                {
                    // エラーハンドリング
                    if (t.IsFaulted)
                    {
                        Debug.LogError($"Failed to build program {programId} for season {season}. Error: {t.Exception.InnerException.Message}");
                    }
                    // セマフォの解放
                    semaphore.Release();
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }

        // 指定プログラムでビルド
        // MEMO:ハンドラ構築前に ProgramObjectQuery.SetupConditions が実行されている必要がある
        public async Task BuildOneProgram(string season, string programId)
        {
            var stringToIdMap = new Dictionary<string, ushort>();
            var idToStringMap = new Dictionary<ushort, string>();
            ushort nextId = 0;
            stringToIdMap[EmptyElementIdKey] = nextId;
            idToStringMap[nextId] = null;
            nextId++;

            // そのシーズンで利用可能性のある全ての要素IDを辞書に登録
            foreach (var elementId in _allPossibleElementIds[season].OrderBy(id => id)) // 順序を安定させるためにソート
            {
                if (!stringToIdMap.ContainsKey(elementId))
                {
                    stringToIdMap[elementId] = nextId;
                    idToStringMap[nextId] = elementId;
                    nextId++;
                }
            }

            var buildResult = await Task.Run(() =>
            {
                return Build(
                    season,
                    _programObjectsMap[season][programId],
                    _programComponentRegulationObjectsMap[season][_programObjectsMap[season][programId].data.programComponentRegulationId],
                    _elementPlaceableSetObjects,
                    _programComponentRegulationArray[season],
                    _elementPlaceableSetArray,
                    _elementPlaceableArray,
                    _elementBaseValueArray[season],
                    stringToIdMap
                );
            });

            // ビルド結果をバイナリファイルに書き出します。
            if (buildResult != null && buildResult.Root.Children.Count > 0)
            {
                var outputDir = buildResult.OutputPath;
                Directory.CreateDirectory(outputDir);

                string binFilePath = null;
                try
                {
                    // IDマップをJSONとして保存
                    var mapJsonPath = Path.Combine(outputDir, "IdMap.json");
                    var json = JsonUtility.ToJson(new ValidProgramComponentsLoader.SerializableUshortStringMap(idToStringMap), true);
                    File.WriteAllText(mapJsonPath, json);

                    // Trie木をGZipで圧縮して保存
                    binFilePath = Path.Combine(outputDir, "ValidProgramComponents.bin.gz");

                    using (var fileStream = new FileStream(binFilePath, FileMode.Create))
                    using (var compressionStream = new GZipStream(fileStream, CompressionMode.Compress))
                    using (var binWriter = new BinaryWriter(compressionStream))
                    {
                        // 圧縮ストリームに直接Trieを書き込む
                        WriteNode(binWriter, buildResult.Root);
                    }
                    Debug.Log($"Successfully wrote GZip compressed Trie data to {binFilePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to write build result to {binFilePath}. Error: {ex.Message}");
                }
            }
            else if (buildResult != null)
            {
                Debug.LogWarning($"No valid program components found for program {programId} in season {season}. Nothing to write.");
            }
            else
            {
                Debug.LogError($"Build failed for program {programId} in season {season}.");
            }
        }

        // Trie木を再帰的にバイナリへ書き込むヘルパーメソッド
        private void WriteNode(BinaryWriter writer, ValidProgramComponents node)
        {
            writer.Write(node.IsEndOfValid);
            if (node.IsEndOfValid)
            {
                writer.Write(node.TotalBaseValue);
            }

            writer.Write(node.Children.Count);
            foreach (var child in node.Children)
            {
                writer.Write(child.Key);
                WriteNode(writer, child.Value);
            }
        }

        private BuildResult Build(
            string season,
            ProgramObject programObject,
            ProgramComponentRegulationObject programComponentRegulationObject,
            List<ElementPlaceableSetObject> elementPlaceableSetObjects,
            ProgramComponentRegulation[] programComponentRegulationArray,
            ElementPlaceableSet[] elementPlaceableSetArray,
            ElementPlaceable[] elementPlaceableArray,
            ElementBaseValue[] elementBaseValueArray,
            Dictionary<string, ushort> stringToIdMap
            )
        {
            var programId = programObject.data.id;
            BuildResult buildResult = new()
            {
                OutputPath = Path.Combine(_outputPath, season, programId)
            };

            // プログラム構成の配置条件を考慮して設定するためのハンドラの用意
            var handler = new ProgramComponentHandler();
            handler.Initialize(
                programObject.data,
                ProgramComponentQuery.Create(
                    programComponentRegulationObject.data,
                    elementPlaceableSetObjects
                ),
                programComponentRegulationArray,
                elementPlaceableSetArray,
                elementPlaceableArray
            );

            // 再帰的に有効なすべての組み合わせを探し出す
            void FindValidCombinationsAndBuildTrie(int componentIndex, int elementIndex, ValidProgramComponents currentNode)
            {
                // ベースケース: すべての要素スロットを調べ終えたら、最終的な検証を行います。
                if (componentIndex >= handler.ProgramComponents.Length)
                {
                    handler.UpdateError();
                    if (handler.Error == null)
                    {
                        currentNode.IsEndOfValid = true;
                        currentNode.TotalBaseValue = ProgramUtility.EstimateTotalBaseValue(
                            handler.Program,
                            handler.ProgramComponents,
                            handler.ElementPlaceableSetAll,
                            elementBaseValueArray,
                            goe: 0
                        );
                    }
                    return;
                }

                var nextComponentIndex = componentIndex;
                var nextElementIndex = elementIndex + 1;
                if (nextElementIndex >= handler.ProgramComponents[componentIndex].elementIds.Length)
                {
                    nextComponentIndex++;
                    nextElementIndex = 0;
                }

                // 1. 現在のスロットを「空」にした場合の組み合わせを試します。
                var emptyElementIdForHandler = (string)null;
                handler.ProgramComponents[componentIndex].elementIds[elementIndex] = emptyElementIdForHandler;
                var emptyId = stringToIdMap[EmptyElementIdKey];

                if (!currentNode.Children.ContainsKey(emptyId))
                {
                    currentNode.Children[emptyId] = new();
                }
                FindValidCombinationsAndBuildTrie(nextComponentIndex, nextElementIndex, currentNode.Children[emptyId]);

                // 2. 現在のスロットに設定可能なすべての要素を試します。
                var currentComponent = handler.ProgramComponents[componentIndex];
                var placeableSet = Array.Find(elementPlaceableSetArray, x => x.id == currentComponent.elementPlaceableSetId);
                var placeable = Array.Find(elementPlaceableArray, x => x.id == placeableSet.elementPlaceableIds[elementIndex]);

                foreach (var elementId in placeable.elementIds)
                {
                    handler.ProgramComponents[componentIndex].elementIds[elementIndex] = elementId;
                    var id = stringToIdMap[elementId];

                    if (!currentNode.Children.ContainsKey(id))
                    {
                        currentNode.Children[id] = new();
                    }
                    FindValidCombinationsAndBuildTrie(nextComponentIndex, nextElementIndex, currentNode.Children[id]);
                }

                // バックトラック
                handler.ProgramComponents[componentIndex].elementIds[elementIndex] = null;
            }
            FindValidCombinationsAndBuildTrie(0, 0, buildResult.Root);

            return buildResult;
        }

        private class BuildResult
        {
            public string OutputPath;
            public ValidProgramComponents Root = new();
        }

        const string EmptyElementIdKey = "$EMPTY_ELEMENT$";

        private readonly string _outputPath;
        private readonly List<ElementPlaceableObject> _elementPlaceableObjects;
        private readonly List<ElementPlaceableSetObject> _elementPlaceableSetObjects;
        private readonly List<EventObject> _eventObjects;
        private readonly List<ElementObject> _elementObjects;
        private readonly Dictionary<string, Dictionary<string, ProgramComponentRegulationObject>> _programComponentRegulationObjectsMap = new();
        private readonly Dictionary<string, Dictionary<string, ProgramObject>> _programObjectsMap = new();
        private readonly ElementPlaceable[] _elementPlaceableArray;
        private readonly ElementPlaceableSet[] _elementPlaceableSetArray;
        private readonly Dictionary<string, ProgramComponentRegulation[]> _programComponentRegulationArray = new();
        private readonly Dictionary<string, Dictionary<string, ElementBaseValueObject>> _elementBaseValueObjectsMap = new();
        private readonly Dictionary<string, ElementBaseValue[]> _elementBaseValueArray = new();
        private readonly Dictionary<string, List<string>> _allPossibleElementIds = new();

        // MEMO:UnityEngine.Addressables以下のAPIをEditor拡張上で使うと不安定だったので、
        //      LoaderUtility.LoadAssetsAsyncのAssetDatabaseをここに独自に用意
        //      本当はLoaderUtility.LoadAssetsAsyncの中でUNITY_EDITORで切り分けたかったが、
        //      UnityEditor.AddressableAssetsのアセンブリが必要になってしまうのでこちらに閉じ込める
        public List<T> LoadAssets<T>(string path) where T : UnityEngine.Object
        {
            var filelistKey = path + "/filelist.txt";
            var filelistObj = LoadAssetByAddress<TextAsset>(filelistKey);
            if (filelistObj == null) return null;
            var assets = filelistObj.text
                .Split("\n")
                .Where(x => !string.IsNullOrEmpty(x));
            if (assets.Count() == 0) return null;
            var results = new List<T>(assets.Count());
            foreach (var asset in assets)
            {
                results.Add(LoadAssetByAddress<T>(asset));
            }
            return results;
        }
        public T LoadAssetByAddress<T>(string address) where T : UnityEngine.Object
        {
            foreach (var entry in _addressableAssetEntry)
            {
                if (entry.address == address)
                {
                    return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(entry.guid));
                }
            }
            return null;
        }
        private readonly List<AddressableAssetEntry> _addressableAssetEntry = new();
    }
}
#endif