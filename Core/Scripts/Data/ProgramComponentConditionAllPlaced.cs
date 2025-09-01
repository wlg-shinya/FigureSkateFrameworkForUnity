using System;
using System.Collections.Generic;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // すべて設定済みか判断する
    [Serializable]
    public class ProgramComponentConditionAllPlaced : ProgramComponentCondition
    {
        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            falseComponentIndexList = new List<int>(); // 新しいリストを作成
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].elementIds.Any(string.IsNullOrEmpty))
                {
                    falseComponentIndexList.Add(i);
                }
            }
            return falseComponentIndexList.Count == 0;
        }
    }
}