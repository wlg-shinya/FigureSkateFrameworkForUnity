#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact.Editor
{
    public class ScriptableObjectFactory : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // マスターデータ(csv)から ScriptableObject の作成
            foreach (var path in importedAssets)
            {
                // データの基になっているソースを記載
                // TODO:pdfをリポジトリに含めるかどうか検討

                // クラス
                // # 2022-23
                // - JSF-Data/日本国内クラスの年齢・取得級/2022-2023度クラス早見表.pdf
                // # 2023-24
                // - JSF-Data/日本国内クラスの年齢・取得級/2023-2024年度クラス早見表.pdf
                // # 2024-25
                // - JSF-Data/日本国内クラスの年齢・取得級/2024-25シーズン 参加資格(年齢・取得級).pdf
                // # 2025-26
                // - JSF-Data/日本国内クラスの年齢・取得級/2025-26シーズン 参加資格(年齢・取得級).pdf
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "Class.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<Class>(rows); },
                        (Class data) => { return $"{data.id}.asset"; },
                        (Class data, ClassObject obj) => { obj.data = data; }
                        );
                }
                // 大会
                // TODO:2022-23のソースのファイル保存
                // TODO:2023-24のソースのファイル保存
                // TODO:2024-25のソースのファイル保存
                // # 国際大会 
                // - https://current.isu.org/figure-skating/entries-results/fsk-results
                // # 国内大会
                // ## 2022-23 
                // - https://www.jsfresults.com/National/2022-2023/fs_j/index.htm
                // ## 2023-24
                // - https://www.jsfresults.com/National/2023-2024/fs_j/index.htm
                // ## 2024-25
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
                // 構成要素基礎点
                // # 2022-23
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2022-2023シーズン/2475_SP_SOV_2022-23.pdf
                // # 2023-24 
                // - 2022-23 と同様
                // # 2024-25 
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2024-2025シーズン/2656 ISU_SOV_SinglesPairs_2024-25_final.pdf
                // # 2025-26
                // - 2025-26 と同様
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "ElementBaseValue.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<ElementBaseValue>(rows); },
                        (ElementBaseValue data) => { return $"{StringWidthConverter.ConvertToFullWidth(data.id)}.asset"; },
                        (ElementBaseValue data, ElementBaseValueObject obj) => { obj.data = data; }
                        );
                }
                // GOE加点・減点項目
                // # 2022-23
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2022-2023シーズン/2474 SP 更新Levels of Difficulty and Guidelines for marking Grade of Execution and Program Components.pdf
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2022-2023シーズン/2474　価値尺度（SOV),難度レベル（ＬＯＤ），ＧＯＥ採点のガイドライン.pdf
                // # 2023-24
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2023-2024シーズン/2558 SP Levels and GOE Season 2023-2024_revJune 9.pdf
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2023-2024シーズン/2558（20230818改訂版）SP難度レベルLOD)、GOEおよびプログラムコンポーネンツ採点のガイドライン2023-24.pdf
                // # 2024-25
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2024-2025シーズン/2623 SP Levels and GOE Season 2024-2025_revJune 24 post congress.pdf
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2024-2025シーズン/Comm. 2623 Rev.2(和訳）_240918.pdf
                // # 2025-26
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/01 シングル・ペア/2025-2026シーズン/Comm. 2701(和訳) SP Levels and GOE Season 2025-26 (202506).pdf
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "GoePlus.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<GoePlus>(rows); },
                        (GoePlus data) => { return $"{data.id}.asset"; },
                        (GoePlus data, GoePlusObject obj) => { obj.data = data; }
                        );
                    // TODO:"SP:要件を満たさないジャンプ要素の最終GOEは必ず"をどう表現するか
                    // TODO:転倒は現在プログラム的には決め打ち実装。データとどう連携するか検討
                    // TODO:「ジャンプのコンボ／シークェンスで複数の“q”」の"複数の“q”"の判定方法の検討
                    // TODO:ジャンプのコンボ／シークェンス中のみに発生するエラーの判定方法の検討
                    // TODO:SP/FSどちらかでしか発生しないエラーの判定方法の検討
                    // TODO:2024-25シーズン以降は、特定GOE減点項目(grey background)に引っかかった場合GOE+2を上限にする
                    // TODO:2024-25シーズン以降の"*が付与されたジャンプ"の扱いについて検討
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
                                    category = rows[rowsIndex][Array.IndexOf(rows[0], "category")],
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
                // プログラム、プログラム構成規則
                // # 2022-23
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/特別規定、技術規定/2022 Special Regulation SP and Ice Dance and Technical Rules SP  and ID_Final.pdf
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/特別規定、技術規定/2022 特別規定.技術規定　シングル&ペア_アイスダンス.pdf
                // - JSF-Data/11 JSF 国内規程/ノービス国内規程（シングル）/過去のデータ/2022-23ノービス課題.pdf
                //   - PCS係数がここからシニア/ジュニアに準拠するようになり以降明記なし
                //   - ジャンプボーナスに関する規則はここ以降明記なし
                //
                // # 2023-24
                // - ノービス以外は 2022-23 のソース内にこのシーズン分も記載あり
                // - JSF-Data/11 JSF 国内規程/ノービス国内規程（シングル）/過去のデータ/2023-24ノービス課題.pdf
                //
                // # 2024-25
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/特別規定、技術規定/2024_Special_Regulation_SP_and_Ice_Dance_and_Technical_Rules_SP__and_ID_Final_rev.pdf
                // - JSF-Data/01 ISU特別規定、技術規定、コミュニケーション/特別規定、技術規定/2024 特別規程、技術規程 シングル＆ペアおよびアイスダンス.pdf
                // - JSF-Data/11 JSF 国内規程/ノービス国内規程（シングル）/2024-25 JSF Nvルール.pdf
                //
                // # 2025-26
                // - ノービス以外は 2024-25 のソース内にこのシーズン分も記載あり
                // - TODO:ノービスの国内ルールが2025/7/1時点で非公開なので公開次第の対応
                {
                    CreateOrUpdateScriptableObjectFromCsv(
                        path,
                        "Program.csv",
                        (List<string[]> rows) => { return CSVSerializer.Deserialize<Program>(rows); },
                        (Program data) => { return $"{data.id}.asset"; },
                        (Program data, ProgramObject obj) => { obj.data = data; }
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
                            var result = new Core.Event[rows.Count - 1];
                            for (var i = 0; i < result.Length; ++i)
                            {
                                var rowsIndex = i + 1;
                                result[i] = new Core.Event
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
                        (Core.Event data) => { return $"{data.id}.asset"; },
                        (Core.Event data, EventObject obj) => { obj.data = data; }
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
                    Path.GetFileNameWithoutExtension(importedAssetPath)
                    );
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

#if UNITY_EDITOR_WIN
        [InitializeOnLoadMethod]
        static void OnProjectLoadedInEditor()
        {
            // ScriptableObject があるディレクトリ以下を一括で読み込むためのファイルリストの作成
            {
                var dirList = new List<string>()
                {
                    "Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Element",
                    "Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/ElementPlaceable",
                    "Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/ElementPlaceableSet",
                    "Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Event",
                    "Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Sex",
                };
                foreach (var season in FactConstant.SEASONS)
                {
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/Class");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/Competition");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/ElementBaseValue");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/GoeMinus");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/GoePlus");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/Program");
                    dirList.Add($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{season}/ProgramComponentRegulation");
                }
                foreach (var dir in dirList)
                {
                    // 対象ディレクトリが存在しない場合はスキップ
                    if (!Directory.Exists(dir)) continue;

                    // ディレクトリ以下にある.assetファイルを列挙
                    var filepathArray = Directory.GetFiles(dir);
                    var assetsArray = filepathArray
                        // .assetファイルのみに絞る
                        .Where(x => Path.GetExtension(x).Equals(".asset"))
                        // Addressablesのキーは'/'しか有効じゃないので'/'に統一
                        .Select(x => x.Replace(@"\", "/"));

                    // テキストファイルにassetパスを書き出す
                    var outputFilePath = Path.Combine(dir, "filelist.txt");
                    var fileContents = string.Join("\n", assetsArray);
                    File.WriteAllText(outputFilePath, fileContents);
                }
            }
        }
#endif
    }
}
#endif