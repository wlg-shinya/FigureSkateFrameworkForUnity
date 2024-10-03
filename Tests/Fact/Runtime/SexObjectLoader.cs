using System.Collections.Generic;
using UnityEditor;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Tests.Fact
{
    // 性別オブジェクトの読み込み
    public static class SexObjectLoader
    {
        // 全オブジェクトを得る
        public static List<SexObject> All()
        {
            return new List<SexObject>
            {
                AssetDatabase.LoadAssetAtPath<SexObject>("Packages/com.welovegamesinc.figureskate/Fact/Objects/Sex/Men.asset"),
                AssetDatabase.LoadAssetAtPath<SexObject>("Packages/com.welovegamesinc.figureskate/Fact/Objects/Sex/Women.asset"),
            };
        }
    }
}
