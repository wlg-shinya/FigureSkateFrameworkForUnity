#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact.Editor
{
    // TODO:ScriptableObjectFactory で作成したデータ自体のテスト追加
    // TODO:可能な限りすべてのデータをcsv->assetの流れにする
    public class ScriptableObjectFactory : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "ElementBaseValue.csv",
                    (ElementBaseValue data) => { return $"{StringWidthConverter.ConvertToFullWidth(data.id)}.asset"; },
                    (ElementBaseValue data, ElementBaseValueObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Element.csv",
                    (Element data) => { return $"{data.id}.asset"; },
                    (Element data, ElementObject obj) => { obj.data = data; }
                    );
                CreateOrUpdateScriptableObjectFromCsv(
                    path,
                    "Class.csv",
                    (Class data) => { return $"{data.id}.asset"; },
                    (Class data, ClassObject obj) => { obj.data = data; }
                    );
                // TODO:CSVSerializerでYearMonthDay(ユーザー定義型)の復元をできるようにする
                // CreateOrUpdateScriptableObjectFromCsv(
                //     path,
                //     "Competition.csv",
                //     (Competition data) => { return $"{data.id}.asset"; },
                //     (Competition data, CompetitionObject obj) => { obj.data = data; }
                //     );
            }
        }

        private delegate string OutputFileName<T>(T data);
        private delegate void SetScriptableObjectData<T, TObject>(T data, TObject obj);
        private static void CreateOrUpdateScriptableObjectFromCsv<T, TObject>(
            string importedAssetPath,
            string csv,
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
                var dataArray = CSVSerializer.Deserialize<T>(csvTextAsset.text);
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