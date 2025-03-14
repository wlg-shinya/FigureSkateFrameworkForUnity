using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact.Utility;

namespace Wlg.FigureSkate.Fact.Query
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

        // 指定オブジェクト群から指定クラスIDと性別IDに一致するオブジェクト単体を得る
        public static EventObject ByClassIdAndSexId(List<EventObject> src, string classId, string sexId)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.classId, classId) && Equals(x.data.sexId, sexId)) ?? throw new Exception($"Not found '{classId}' or '{sexId}'");
        }
    }
}
