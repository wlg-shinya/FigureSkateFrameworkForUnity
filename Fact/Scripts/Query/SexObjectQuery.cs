using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    // 性別オブジェクトを得るための問い合わせ
    public static class SexObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<SexObject>> All()
        {
            return await LoaderUtility.LoadAssetsAsync<SexObject>(@"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Sex");
        }

        // 指定IDのオブジェクトを得る
        public static SexObject ById(List<SexObject> src, string id) => src.Find(x => x.data.id == id) ?? throw new Exception($"Not found '{id}'");
    }
}
