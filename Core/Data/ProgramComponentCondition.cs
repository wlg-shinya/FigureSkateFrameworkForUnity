using System;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム構成全体に対する設定可能な構成要素の条件
    [Serializable]
    public abstract class ProgramComponentCondition
    {
        // 条件を満たしていないときのメッセージ
        public string falseMessage;
        // 条件
        public abstract bool Condition(ProgramComponent[] components);
    }
}