using System;
using System.Collections.Generic;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // 構成要素オブジェクトを得るための問い合わせ
    public static class ElementObjectQuery
    {
        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ElementObject ById(List<ElementObject> src, string id)
        {
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }
    }
}
