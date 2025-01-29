#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact.Editor
{
    // TODO:Objects以下のファイルのAddressable設定をフレームワーク側で用意する
    public class ScriptableObjectFactory : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "ElementBaseValue.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<ElementBaseValue>(rows); },
                    (ElementBaseValue data) => { return $"{StringWidthConverter.ConvertToFullWidth(data.id)}.asset"; },
                    (ElementBaseValue data, ElementBaseValueObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Element.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<Element>(rows); },
                    (Element data) => { return $"{data.id}.asset"; },
                    (Element data, ElementObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Class.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<Class>(rows); },
                    (Class data) => { return $"{data.id}.asset"; },
                    (Class data, ClassObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Sex.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<Sex>(rows); },
                    (Sex data) => { return $"{data.id}.asset"; },
                    (Sex data, SexObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Event.csv",
                    (List<string[]> rows) =>
                    {
                        // 配列(programIds)を扱うため独自に値を設定する
                        var result = new Core.Data.Event[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new Core.Data.Event
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                name = rows[rowsIndex][Array.IndexOf(rows[0], "name")],
                                classId = rows[rowsIndex][Array.IndexOf(rows[0], "classId")],
                                sexId = rows[rowsIndex][Array.IndexOf(rows[0], "sexId")],
                                programIds = rows[rowsIndex][Array.IndexOf(rows[0], "programIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (Core.Data.Event data) => { return $"{data.id}.asset"; },
                    (Core.Data.Event data, EventObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Competition.csv",
                    (List<string[]> rows) =>
                    {
                        // ユーザー型(YearMonthDay)と配列(eventIds)を扱うため独自に値を設定する
                        var result = new Competition[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new Competition
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                regionId = rows[rowsIndex][Array.IndexOf(rows[0], "regionId")],
                                countryId = rows[rowsIndex][Array.IndexOf(rows[0], "countryId")],
                                name = rows[rowsIndex][Array.IndexOf(rows[0], "name")],
                                isInternational = bool.Parse(rows[rowsIndex][Array.IndexOf(rows[0], "isInternational")]),
                                startDay = new YearMonthDay(rows[rowsIndex][Array.IndexOf(rows[0], "startDay")]),
                                endDay = new YearMonthDay(rows[rowsIndex][Array.IndexOf(rows[0], "endDay")]),
                                eventIds = rows[rowsIndex][Array.IndexOf(rows[0], "eventIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (Competition data) => { return $"{data.id}.asset"; },
                    (Competition data, CompetitionObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Goe.csv",
                    (List<string[]> rows) =>
                    {
                        // 配列(plusIds,minusIds)を扱うため独自に値を設定する
                        var result = new Goe[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new Goe
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                plusIds = rows[rowsIndex][Array.IndexOf(rows[0], "plusIds")].Split('/'),
                                minusIds = rows[rowsIndex][Array.IndexOf(rows[0], "minusIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (Goe data) => { return $"{data.id}.asset"; },
                    (Goe data, GoeObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "GoePlus.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<GoePlus>(rows); },
                    (GoePlus data) => { return $"{data.id}.asset"; },
                    (GoePlus data, GoePlusObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "GoeMinus.csv",
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<GoeMinus>(rows); },
                    (GoeMinus data) => { return $"{data.id}.asset"; },
                    (GoeMinus data, GoeMinusObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "ElementPlaceable.csv",
                    (List<string[]> rows) =>
                    {
                        // 配列(elementIds)を扱うため独自に値を設定する
                        var result = new ElementPlaceable[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new ElementPlaceable
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                elementIds = rows[rowsIndex][Array.IndexOf(rows[0], "elementIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (ElementPlaceable data) => { return $"{data.id}.asset"; },
                    (ElementPlaceable data, ElementPlaceableObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "ElementPlaceableSet.csv",
                    (List<string[]> rows) =>
                    {
                        // 配列(elementPlaceableIds)を扱うため独自に値を設定する
                        var result = new ElementPlaceableSet[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new ElementPlaceableSet
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                name = rows[rowsIndex][Array.IndexOf(rows[0], "name")],
                                jump = bool.Parse(rows[rowsIndex][Array.IndexOf(rows[0], "jump")]),
                                elementPlaceableIds = rows[rowsIndex][Array.IndexOf(rows[0], "elementPlaceableIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (ElementPlaceableSet data) => { return $"{data.id}.asset"; },
                    (ElementPlaceableSet data, ElementPlaceableSetObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "ProgramComponentRegulation.csv",
                    (List<string[]> rows) =>
                    {
                        // 配列(elementIds)を扱うため独自に値を設定する
                        var result = new ProgramComponentRegulation[rows.Count - 1];
                        for (var i = 0; i < result.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            result[i] = new ProgramComponentRegulation
                            {
                                id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                elementPlaceableSetIds = rows[rowsIndex][Array.IndexOf(rows[0], "elementPlaceableSetIds")].Split('/'),
                            };
                        }
                        return result;
                    },
                    (ProgramComponentRegulation data) => { return $"{data.id}.asset"; },
                    (ProgramComponentRegulation data, ProgramComponentRegulationObject obj) => { obj.data = data; }
                    );
                // TODO:Program のcsv化のため、ProgramComponentRegulationデータの扱いを変更する
                // CreateOrUpdateScriptableObjectFromCsv(
                //     path,
                //     "Program.csv",
                //     (Program data) => { return $"{data.id}.asset"; },
                //     (Program data, ProgramObject obj) => { obj.data = data; }
                //     );
            }
        }

        private delegate T[] CsvParseToDeserialize<T>(List<string[]> rows);
        private delegate string OutputFileName<T>(T data);
        private delegate void SetScriptableObjectData<T, TObject>(T data, TObject obj);
        private static void CreateOrUpdateScriptableObjectFromCsv<T, TObject>(
            string importedAssetPath,
            string csv,
            CsvParseToDeserialize<T> csvParseToDeserialize,
            OutputFileName<T> outputFileName,
            SetScriptableObjectData<T, TObject> setScriptableObjectData)
            where TObject : ScriptableObject
        {
            if (importedAssetPath.IndexOf($"/{csv}") != -1)
            {
                // 保存先ディレクトリはcsvデータ置き場の構造を参考にする
                var outputDir = Path.Combine(
                    Path.GetDirectoryName(importedAssetPath).Replace("MasterData", "Objects"),
                    Path.GetFileNameWithoutExtension(importedAssetPath));
                // csvからデータ読み込み
                var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(importedAssetPath);
                var dataArray = csvParseToDeserialize(CSVSerializer.ParseCSV(csvTextAsset.text));
                foreach (var data in dataArray)
                {
                    // 保存先ファイルパスを得る
                    var outputFilePath = Path.Combine(outputDir, outputFileName(data));
                    // 既存のScriptableObjectの読み込み。存在しない場合は新規作成
                    TObject obj = AssetDatabase.LoadAssetAtPath<TObject>(outputFilePath);
                    if (obj == null)
                    {
                        obj = ScriptableObject.CreateInstance<TObject>();
                        AssetDatabase.CreateAsset(obj, outputFilePath);
                    }
                    // ScriptableObjectにデータを設定
                    setScriptableObjectData(data, obj);
                    // 変更した結果をファイル保存
                    EditorUtility.SetDirty(obj);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
}
#endif