#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact.Editor
{
    public class ValidProgramComponentsBuilder
    {
        public ValidProgramComponentsBuilder(string outputPath)
        {
            _outputPath = outputPath;
        }

        public async Task Initialize()
        {
            // Addressabless初期化完了を待機
            await Addressables.InitializeAsync().Task;

            // フィギュアスケートデータの読み込み
            var taskEventObjectQueryAll = EventObjectQuery.All();
            var taskElementPlaceableSetObjectQueryAll = ElementPlaceableSetObjectQuery.All();
            var taskElementPlaceableObjectQueryAll = ElementPlaceableObjectQuery.All();
            var taskElementObjectQueryAll = ElementObjectQuery.All();
            await Task.WhenAll(
                taskEventObjectQueryAll,
                taskElementPlaceableSetObjectQueryAll,
                taskElementPlaceableObjectQueryAll,
                taskElementObjectQueryAll
                );
            _eventObjects = taskEventObjectQueryAll.Result;
            _elementPlaceableSetObjects = taskElementPlaceableSetObjectQueryAll.Result;
            _elementPlaceableObjects = taskElementPlaceableObjectQueryAll.Result;
            _elementObjects = taskElementObjectQueryAll.Result;
            _elementPlaceableSetArray = _elementPlaceableSetObjects.Select(x => x.data).ToArray();
            _elementPlaceableArray = _elementPlaceableObjects.Select(x => x.data).ToArray();
            foreach (var season in FactConstant.SEASONS)
            {
                var seasonStartDay = YearMonthDayUtility.GetSeasonStartDay(season);
                var taskProgramObjectQueryAll = ProgramObjectQuery.All(seasonStartDay);
                var taskProgramComponentRegulationObjectQueryAll = ProgramComponentRegulationObjectQuery.All(seasonStartDay);
                var taskElementBaseValueObjectQueryAll = ElementBaseValueObjectQuery.All(seasonStartDay);
                await Task.WhenAll(
                    taskProgramObjectQueryAll,
                    taskProgramComponentRegulationObjectQueryAll,
                    taskElementBaseValueObjectQueryAll
                    );
                _programObjects.Add(season, taskProgramObjectQueryAll.Result.ToDictionary(x => x.data.id, x => x));
                _programComponentRegulationObjects.Add(season, taskProgramComponentRegulationObjectQueryAll.Result);
                _programComponentRegulationArray.Add(season, taskProgramComponentRegulationObjectQueryAll.Result.Select(x => x.data).ToArray());
                _elementBaseValueObjectsMap.Add(season, taskElementBaseValueObjectQueryAll.Result.ToDictionary(x => x.data.id, x => x));
                _elementBaseValueArray.Add(season, taskElementBaseValueObjectQueryAll.Result.Select(x => x.data).ToArray());

                // プログラム情報に配置可能な条件をセットアップして差し替え
                foreach (var eventObject in _eventObjects)
                {
                    var setupedProgramObjects = ProgramObjectQuery
                        .ByIds(_programObjects[season].Values.ToList(), eventObject.data.programIds)
                        .Select(x => ProgramObjectQuery.SetupConditions(x, _programComponentRegulationObjects[season], _elementPlaceableSetObjects))
                        .ToList();
                    foreach (var setupedProgramObject in setupedProgramObjects)
                    {
                        _programObjects[season][setupedProgramObject.data.id] = setupedProgramObject;
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
            // 全シーズンの全プログラム構成に対して構成可能な要素の組み割合わせを出力する
            var taskList = new List<Task>();
            foreach (var season in FactConstant.SEASONS)
            {
                foreach (var programObject in _programObjects[season].Values)
                {
                    taskList.Add(BuildByProgram(season, programObject.data.id));
                }
            }
            await Task.WhenAll(taskList);
        }

        // 指定プログラムでビルド
        // MEMO:ハンドラ構築前に ProgramObjectQuery.SetupConditions が実行されている必要がある
        public async Task BuildByProgram(string season, string programId)
        {
            await Task.Run(() =>
            {
                var validProgramComponents = new ValidProgramComponents()
                {
                    programId = programId,
                    programComponents = new(),
                    totalBaseValue = 0.0f
                };
                // プログラム構成の配置条件を考慮して設定するためのハンドラの用意
                var handler = new ProgramComponentHandler();
                handler.Initialize(
                    _programObjects[season][programId].data,
                    validProgramComponents.programComponents.components,
                    _programComponentRegulationArray[season],
                    _elementPlaceableSetArray,
                    _elementPlaceableArray
                );

                // TODO:ここでhandler.TrySetを行い、すべて設定してもhandler.Error==nullになる組み合わせをひとつ見つける

                // 構成したプログラムの合計基礎点を記録
                // validProgramComponents.totalBaseValue = ProgramUtility.EstimateTotalBaseValue(
                //     handler.Program,
                //     handler.ProgramComponents,
                //     handler.ElementPlaceableSetAll,
                //     _elementBaseValueArray[season],
                //     goe: 0
                //     );

                // TODO:ここでoutputDir以下にValidProgramComponentsをファイルに書き出す
                var outputDir = Path.Combine(_outputPath, season, programId);
                Debug.Log(outputDir);
            });
        }

        private readonly string _outputPath;
        private List<ElementPlaceableObject> _elementPlaceableObjects;
        private List<ElementPlaceableSetObject> _elementPlaceableSetObjects;
        private List<EventObject> _eventObjects;
        private List<ElementObject> _elementObjects;
        private Dictionary<string, List<ProgramComponentRegulationObject>> _programComponentRegulationObjects = new();
        private Dictionary<string, Dictionary<string, ProgramObject>> _programObjects = new();
        private ElementPlaceable[] _elementPlaceableArray;
        private ElementPlaceableSet[] _elementPlaceableSetArray;
        private Dictionary<string, ProgramComponentRegulation[]> _programComponentRegulationArray = new();
        private Dictionary<string, Dictionary<string, ElementBaseValueObject>> _elementBaseValueObjectsMap = new();
        private Dictionary<string, ElementBaseValue[]> _elementBaseValueArray = new();
        private Dictionary<string, List<string>> _allPossibleElementIds = new();
    }
}
#endif