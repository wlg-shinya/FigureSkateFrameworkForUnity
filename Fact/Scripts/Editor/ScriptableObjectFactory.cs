#if UNITY_EDITOR
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
                // csvからElementObjectを構築
                if (path.IndexOf("/Element/master.csv") != -1)
                {
                    var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                    var elements = CSVSerializer.Deserialize<Element>(csvTextAsset.text);
                    foreach (var element in elements)
                    {
                        // 保存先ファイルパスを得る。ファイル名に使用禁止文字が含まれうるのですべて全角に変換しておく
                        var elementObjectFilePath = @$"{Path.GetDirectoryName(path)}/{StringWidthConverter.ConvertToFullWidth(element.id)}.asset";
                        // 既存のScriptableObjectの読み込み。存在しない場合は新規作成
                        ElementObject elementObject = AssetDatabase.LoadAssetAtPath<ElementObject>(elementObjectFilePath);
                        if (elementObject == null)
                        {
                            elementObject = ScriptableObject.CreateInstance<ElementObject>();
                            AssetDatabase.CreateAsset(elementObject, elementObjectFilePath);
                        }
                        // ScriptableObjectにデータを設定
                        elementObject.data = element;
                        // 変更した結果をファイル保存
                        EditorUtility.SetDirty(elementObject);
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}
#endif