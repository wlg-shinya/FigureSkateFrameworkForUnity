using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // GOE加点項目オブジェクトを得るための問い合わせ
    public static class GoePlusObjectQuery
    {
        // 基準日の年度の全オブジェクトを読み込む
        public static async Task<List<GoePlusObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<GoePlusObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/GoePlus");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static GoePlusObject ById(List<GoePlusObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
