#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact.Editor
{
    // TODO:ScriptableObjectFactory で作成したデータ自体のテスト追加
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
                    (List<string[]> rows) => { return CSVSerializer.Deserialize<Core.Data.Event>(rows); },
                    (Core.Data.Event data) => { return $"{data.id}.asset"; },
                    (Core.Data.Event data, EventObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Competition.csv",
                    (List<string[]> rows) =>
                    {
                        // ユーザー型(YearMonthDay)を扱うため CSVSerializer.Deserialize だと都合が悪いので独自に値を設定する
                        // ヘッダ部を無視するために確保する配列数とイテレーションするインデックス数に気を付ける
                        var competitions = new Competition[rows.Count - 1];
                        for (var i = 0; i < competitions.Length; ++i)
                        {
                            var rowsIndex = i + 1;
                            competitions[i] = new Competition
                            {
                                id = rows[rowsIndex][0],
                                regionId = rows[rowsIndex][1],
                                countryId = rows[rowsIndex][2],
                                name = rows[rowsIndex][3],
                                isInternational = bool.Parse(rows[rowsIndex][4]),
                                startDay = new YearMonthDay(rows[rowsIndex][5]),
                                endDay = new YearMonthDay(rows[rowsIndex][6]),
                            };
                        }
                        return competitions;
                    },
                    (Competition data) => { return $"{data.id}.asset"; },
                    (Competition data, CompetitionObject obj) => { obj.data = data; }
                    );
                // TODO:Goe のcsv化のため、GoePlus/GoeMinusデータの扱いを変更する
                // CreateOrUpdateScriptableObjectFromCsv(
                //     path,
                //     "Goe.csv",
                //     (Goe data) => { return $"{data.id}.asset"; },
                //     (Goe data, GoeObject obj) => { obj.data = data; }
                //     );
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