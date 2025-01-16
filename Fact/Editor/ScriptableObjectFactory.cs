#if UNITY_EDITOR
using System.IO;
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
                if (path.IndexOf("/ElementBaseValue.csv") != -1)
                {
                    var outputDir = Path.Combine(Path.GetDirectoryName(path).Replace("MasterData", "Objects"), Path.GetFileNameWithoutExtension(path));
                    var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    var elements = CSVSerializer.Deserialize<ElementBaseValue>(csvTextAsset.text);
                    foreach (var element in elements)
                    {
                        // 保存先ファイルパスを得る。ファイル名に使用禁止文字が含まれうるのですべて全角に変換しておく
                        var outputFilePath = Path.Combine(outputDir, $"{StringWidthConverter.ConvertToFullWidth(element.id)}.asset");
                        // 既存のScriptableObjectの読み込み。存在しない場合は新規作成
                        ElementBaseValueObject elementObject = AssetDatabase.LoadAssetAtPath<ElementBaseValueObject>(outputFilePath);
                        if (elementObject == null)
                        {
                            elementObject = ScriptableObject.CreateInstance<ElementBaseValueObject>();
                            AssetDatabase.CreateAsset(elementObject, outputFilePath);
                        }
                        // ScriptableObjectにデータを設定
                        elementObject.data = element;
                        // 変更した結果をファイル保存
                        EditorUtility.SetDirty(elementObject);
                        AssetDatabase.SaveAssets();
                    }
                }
                if (path.IndexOf("/Element.csv") != -1)
                {
                    var outputDir = Path.Combine(Path.GetDirectoryName(path).Replace("MasterData", "Objects"), Path.GetFileNameWithoutExtension(path));
                    var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    var elements = CSVSerializer.Deserialize<Element>(csvTextAsset.text);
                    foreach (var element in elements)
                    {
                        // 保存先ファイルパスを得る
                        var outputFilePath = Path.Combine(outputDir, $"{element.id}.asset");
                        ElementObject obj = AssetDatabase.LoadAssetAtPath<ElementObject>(outputFilePath);
                        if (obj == null)
                        {
                            obj = ScriptableObject.CreateInstance<ElementObject>();
                            AssetDatabase.CreateAsset(obj, outputFilePath);
                        }
                        obj.data = element;
                        EditorUtility.SetDirty(obj);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}
#endif