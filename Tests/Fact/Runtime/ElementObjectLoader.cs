using System.Collections.Generic;
using UnityEditor;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact;

namespace Wlg.FigureSkate.Tests.Fact
{
    // 構成要素オブジェクトの読み込み
    public static class ElementObjectLoader
    {
        // 全構成要素ID
        public static string[] AllIds = {
            "1T", "1S", "1Lo", "1F", "1Lz", "1A", "1Eu",
            "2T", "2S", "2Lo", "2F", "2Lz", "2A",
            "3T", "3S", "3Lo", "3F", "3Lz", "3A",
            "4T", "4S", "4Lo", "4F", "4Lz", "4A",
            "SSp4",
            "FSSp4",
            "CSSp4",
            "FCSp4",
            "CoSp4",
            "FCoSp4",
            "CCoSp4",
            "StSqB",
            "ChSq1"
        };

        // 基準日の年度の全オブジェクトを生成する
        public static List<ElementObject> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var list = new List<ElementObject>(AllIds.Length);
            foreach (var id in AllIds)
            {
                list.Add(AssetDatabase.LoadAssetAtPath<ElementObject>($"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/Element/{id}.asset"));
            }
            return list;
        }
    }
}
