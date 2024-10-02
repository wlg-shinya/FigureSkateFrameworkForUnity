using System;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム構成に設定可能な構成要素ひとつを表現したデータ
    [Serializable]
    public class ElementPlaceable
    {
        // 登録可能な構成要素のID
        public string[] elementIds;
    }
}