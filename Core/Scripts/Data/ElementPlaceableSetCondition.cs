using System;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成一つに対する設定可能な構成要素の条件
    [Serializable]
    public abstract class ElementPlaceableSetCondition
    {
        // 条件を満たしていないときのメッセージ
        public string falseMessage;
        // 条件
        public virtual bool Condition(string[] placedElementIds) { return false; }
    }
}