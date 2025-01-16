using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 構成要素の基礎点以外の要素
    // 基礎点は ElementBaseValue で扱う
    [Serializable]
    public class Element
    {
        // 一意の記号
        public string id;
        // 正式名称
        public string name;
        // 対象GOEのID
        public string goeId;
        // 一つ上のグレードの構成要素ID。nullか空文字なら最高グレード
        public string upgradeId;
        // 一つ下のグレードの構成要素ID。nullか空文字なら最低グレード
        public string downgradeId;
    }
}