using System.Collections.Generic;
using UnityEditor;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact;

namespace Wlg.FigureSkate.Tests.Fact
{
    // 大会オブジェクトの読み込み
    public static class CompetitionObjectLoader
    {
        // 基準日の年度の全大会を生成する
        public static List<CompetitionObject> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return new List<CompetitionObject>
            {
                AssetDatabase.LoadAssetAtPath<CompetitionObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Competition/AllJapanNoviceChampionship.asset"),
                AssetDatabase.LoadAssetAtPath<CompetitionObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Competition/ISUJuniorGrandPrixOsaka.asset"),
                AssetDatabase.LoadAssetAtPath<CompetitionObject>($"Packages/com.welovegamesinc.figureskate/Fact/Objects/{skateYear}/Competition/KinoshitaGroupCupJapanOpen2023.asset"),
            };
        }
    }
}
