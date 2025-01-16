using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // 構成要素の基礎点オブジェクトを得るための問い合わせ
    public static class ElementBaseValueObjectQuery
    {
        // 基準日の年度の全オブジェクトを読み込む
        public static async Task<List<ElementBaseValueObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<ElementBaseValueObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/ElementBaseValue");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ElementBaseValueObject ById(List<ElementBaseValueObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
