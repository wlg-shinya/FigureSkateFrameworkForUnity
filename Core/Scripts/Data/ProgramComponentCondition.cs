using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成全体に対する設定可能な構成要素の条件
    [Serializable]
    public abstract class ProgramComponentCondition
    {
        // 条件を満たしていないときのメッセージ
        public LocalizedString falseMessage;
        // 条件
        public abstract bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList);
    }
}