using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact.Utility;

namespace Wlg.FigureSkate.Fact.Query
{
    // プログラム構成の1項目を表現したデータのオブジェクトを得るための問い合わせ
    public static class ProgramComponentRegulationObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<ProgramComponentRegulationObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<ProgramComponentRegulationObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/ProgramComponentRegulation");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ProgramComponentRegulationObject ById(List<ProgramComponentRegulationObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
