using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成に設定可能な構成要素ひとつを表現したデータのオブジェクトを得るための問い合わせ
    public static class ElementPlaceableObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<ElementPlaceableObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<ElementPlaceableObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/ElementPlaceable");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ElementPlaceableObject ById(List<ElementPlaceableObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
