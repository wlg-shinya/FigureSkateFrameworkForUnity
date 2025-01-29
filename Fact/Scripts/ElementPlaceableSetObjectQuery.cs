using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成の1項目を表現したデータのオブジェクトを得るための問い合わせ
    public static class ElementPlaceableSetObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<ElementPlaceableSetObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<ElementPlaceableSetObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/ElementPlaceableSet");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ElementPlaceableSetObject ById(List<ElementPlaceableSetObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
