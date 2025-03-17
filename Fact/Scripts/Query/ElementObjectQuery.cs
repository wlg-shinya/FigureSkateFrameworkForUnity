using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    // 構成要素オブジェクトを得るための問い合わせ
    public static class ElementObjectQuery
    {
        // 全オブジェクトを読み込む
        public static async Task<List<ElementObject>> All()
        {
            return await LoaderUtility.LoadAssetsAsync<ElementObject>(@"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/Element");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ElementObject ById(List<ElementObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
