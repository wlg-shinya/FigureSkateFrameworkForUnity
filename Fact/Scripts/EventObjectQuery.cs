using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // イベントオブジェクトを得るための問い合わせ
    public static class EventObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<EventObject>> All()
        {
            return await LoaderUtility.LoadAssetsAsync<EventObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Event");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static EventObject ById(List<EventObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
