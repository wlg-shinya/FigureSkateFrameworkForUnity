using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    // GOE減点項目オブジェクトを得るための問い合わせ
    public static class GoeMinusObjectQuery
    {
        // 基準日の年度の全オブジェクトを読み込む
        public static async Task<List<GoeMinusObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<GoeMinusObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/GoeMinus");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static GoeMinusObject ById(List<GoeMinusObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
