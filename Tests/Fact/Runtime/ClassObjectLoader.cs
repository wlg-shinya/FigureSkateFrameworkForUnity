using System.Collections.Generic;
using UnityEditor;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;

namespace Wlg.FigureSkate.Tests.Fact
{
    // クラスオブジェクトの読み込み
    public static class ClassObjectLoader
    {
        // 基準日の年度の全オブジェクトを生成する
        public static List<ClassObject> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return new List<ClassObject>
            {
                AssetDatabase.LoadAssetAtPath<ClassObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Class/Senior.asset"),
                AssetDatabase.LoadAssetAtPath<ClassObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Class/Junior.asset"),
                AssetDatabase.LoadAssetAtPath<ClassObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Class/NoviceA.asset"),
                AssetDatabase.LoadAssetAtPath<ClassObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Class/NoviceB.asset"),
                AssetDatabase.LoadAssetAtPath<ClassObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Class/None.asset"),
            };
        }
    }
}
