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
                // csvからElementBaseValueObjectを構築
                if (path.IndexOf("/ElementBaseValue/master.csv") != -1)
                {
                    var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    var elements = CSVSerializer.Deserialize<ElementBaseValue>(csvTextAsset.text);
                    foreach (var element in elements)
                    {
                        // 保存先ファイルパスを得る。ファイル名に使用禁止文字が含まれうるのですべて全角に変換しておく
                        var elementObjectFilePath = @$"{Path.GetDirectoryName(path)}/{StringWidthConverter.ConvertToFullWidth(element.id)}.asset";
                        // 既存のScriptableObjectの読み込み。存在しない場合は新規作成
                        ElementBaseValueObject elementObject = AssetDatabase.LoadAssetAtPath<ElementBaseValueObject>(elementObjectFilePath);
                        if (elementObject == null)
                        {
                            elementObject = ScriptableObject.CreateInstance<ElementBaseValueObject>();
                            AssetDatabase.CreateAsset(elementObject, elementObjectFilePath);
                        }
                        // ScriptableObjectにデータを設定
                        elementObject.data = element;
                        // 変更した結果をファイル保存
                        EditorUtility.SetDirty(elementObject);
                        AssetDatabase.SaveAssets();
                    }
                }
                // csvからElementObjectを構築
                if (path.IndexOf("/Element/master.csv") != -1)
                {
                    var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    var elements = CSVSerializer.Deserialize<Element>(csvTextAsset.text);
                    foreach (var element in elements)
                    {
                        // 保存先ファイルパスを得る
                        var elementObjectFilePath = @$"{Path.GetDirectoryName(path)}/{element.id}.asset";
                        ElementObject elementObject = AssetDatabase.LoadAssetAtPath<ElementObject>(elementObjectFilePath);
                        if (elementObject == null)
                        {
                            elementObject = ScriptableObject.CreateInstance<ElementObject>();
                            AssetDatabase.CreateAsset(elementObject, elementObjectFilePath);
                        }
                        elementObject.data = element;
                        EditorUtility.SetDirty(elementObject);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}
#endif