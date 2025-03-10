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
    public class ScriptableObjectFactory : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                // データの基になっているソースを記載
                // TODO:pdfをリポジトリに含めるかどうか検討

                // # 2022-23 TODO:データ化
                // - 2475_SP_SOV_2022-23.pdf
                // # 2023-24 
                // - 2022-23 と同様
                // # 2024-25 
                // - 2656 ISU_SOV_SinglesPairs_2024-25_final.pdf
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "ElementBaseValue.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<ElementBaseValue>(rows); },
                        (ElementBaseValue data) => { return $"{StringWidthConverter.ConvertToFullWidth(data.id)}.asset"; },
                        (ElementBaseValue data, ElementBaseValueObject obj) => { obj.data = data; }
                        );
                }
                // # 2022-23 TODO:データ化
                // - 2022-2023度クラス早見表.pdf
                // # 2023-24
                // - 2023-2024年度クラス早見表.pdf
                // # 2024-25
                // - 2024-25シーズン 参加資格(年齢・取得級).pdf
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "Class.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<Class>(rows); },
                        (Class data) => { return $"{data.id}.asset"; },
                        (Class data, ClassObject obj) => { obj.data = data; }
                        );
                }
                // # 国際大会 
                // - https://current.isu.org/figure-skating/entries-results/fsk-results
                // TODO: 2022-23のデータ化
                // TODO: 2022-23のソースのファイル保存
                // TODO: 2024-25のデータ化
                // TODO: 2024-25のソースのファイル保存
                // # 国内大会
                // ## 2022-23 
                // TODO:データ化
                // TODO:ソースのファイル保存
                // - https://www.jsfresults.com/National/2022-2023/fs_j/index.htm
                // ## 2023-24
                // TODO:ソースのファイル保存
                // - https://www.jsfresults.com/National/2023-2024/fs_j/index.htm
                // ## 2024-25
                // TODO:データ化
                // TODO:ソースのファイル保存
                // - https://www.jsfresults.com/National/2024-2025/fs_j/index.htm
                {
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
                }
                // # 2022-23 TODO:データ化
                // - 2474 SP 更新Levels of Difficulty and Guidelines for marking Grade of Execution and Program Components.pdf
                // - 2474　価値尺度（SOV),難度レベル（ＬＯＤ），ＧＯＥ採点のガイドライン.pdf
                // # 2023-24
                // - 2558 SP Levels and GOE Season 2023-2024_revJune 9.pdf
                // - 2558（20230818改訂版）SP難度レベルLOD)、GOEおよびプログラムコンポーネンツ採点のガイドライン2023-24.pdf
                // # 2024-25 TODO:データ化
                // - 2623 SP Levels and GOE Season 2024-2025_revJune 24 post congress.pdf
                // - Comm. 2623 Rev.2(和訳）_240918.pdf
                {
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
                    // TODO:"SP:要件を満たさないジャンプ要素の最終GOEは必ず"をどう表現するか(現在はデータとして用意していない)
                    // TODO:転倒は現在プログラム的には決め打ち実装。データとどう連携するか検討
                    // TODO:「ジャンプのコンボ／シークェンスで複数の“q”」の"複数の“q”"の判定方法の検討
                    // TODO:ジャンプのコンボ／シークェンス中のみに発生するエラーの判定方法の検討
                    // TODO:SP/FSどちらかでしか発生しないエラーの判定方法の検討
                    // TODO:2024-25シーズンは、特定GOE減点項目(grey background)に引っかかった場合GOE+2を上限にする
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "GoeMinus.csv",
                        (List<string[]> rows) =>
                        {
                            // 配列(targetElementIds)を扱うため独自に値を設定する
                            var result = new GoeMinus[rows.Count - 1];
                            for (var i = 0; i < result.Length; ++i)
                            {
                                var rowsIndex = i + 1;
                                result[i] = new GoeMinus
                                {
                                    id = rows[rowsIndex][Array.IndexOf(rows[0], "id")],
                                    description = rows[rowsIndex][Array.IndexOf(rows[0], "description")],
                                    group = rows[rowsIndex][Array.IndexOf(rows[0], "group")],
                                    minValue = int.Parse(rows[rowsIndex][Array.IndexOf(rows[0], "minValue")]),
                                    maxValue = int.Parse(rows[rowsIndex][Array.IndexOf(rows[0], "maxValue")]),
                                    mark = rows[rowsIndex][Array.IndexOf(rows[0], "mark")]
                                };
                                // targetElementIds は空データを許容しているので
                                // データがある場合は分割データ、空データの場合は空配列を設定する
                                var targetElementIdsIndex = Array.IndexOf(rows[0], "targetElementIds");
                                result[i].targetElementIds = rows[rowsIndex].Length > targetElementIdsIndex ? rows[rowsIndex][targetElementIdsIndex].Split('/') : new string[] { };
                            }
                            return result;
                        },
                        (GoeMinus data) => { return $"{data.id}.asset"; },
                        (GoeMinus data, GoeMinusObject obj) => { obj.data = data; }
                        );
                }
                // # 2022-23 TODO:データ化
                // ## シニア/ジュニア
                // - 2022 Special Regulation SP and Ice Dance and Technical Rules SP  and ID_Final.pdf
                // - 2022 特別規定.技術規定　シングル&ペア_アイスダンス.pdf
                // ## ノービス
                // - 2022-23ノービス課題.pdf
                //   - PCS係数がここからシニア/ジュニアに準拠するようになり以降明記なし
                //   - ジャンプボーナスに関する規則はここ以降か明記なし
                //
                // # 2023-24
                // - 2022 Special Regulation SP and Ice Dance and Technical Rules SP  and ID_Final.pdf
                // - 2022 特別規定.技術規定　シングル&ペア_アイスダンス.pdf
                //   - プログラム構成ルールは2023-24も上記に含まれている
                //
                // # 2024-25 TODO:データ化
                // - 2024_Special_Regulation_SP_and_Ice_Dance_and_Technical_Rules_SP__and_ID_Final_rev.pdf
                {
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

                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "Program.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<Program>(rows); },
                        (Program data) => { return $"{data.id}.asset"; },
                        (Program data, ProgramObject obj) => { obj.data = data; }
                        );
                }
                // シーズンに属さないデータ
                // 用語についてはこれまでのソース内に散らばっているため、がんばって収集する必要がある
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "Element.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<Element>(rows); },
                        (Element data) => { return $"{data.id}.asset"; },
                        (Element data, ElementObject obj) => { obj.data = data; }
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
                }
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