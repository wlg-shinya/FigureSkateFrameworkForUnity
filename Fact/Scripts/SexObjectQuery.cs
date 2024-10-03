using System;
using System.Collections.Generic;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // 性別オブジェクトを得るための問い合わせ
    public static class SexObjectQuery
    {
        // 指定IDのオブジェクトを得る
        public static SexObject ById(List<SexObject> src, string id) => src.Find(x => x.data.id == id) ?? throw new Exception($"Not found '{id}'");
    }
}
