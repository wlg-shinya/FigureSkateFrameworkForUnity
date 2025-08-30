using System;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // すべて設定済みか判断する
    [Serializable]
    public class ProgramComponentConditionAllPlaced : ProgramComponentCondition
    {
        public override bool Condition(ProgramComponent[] components)
        {
            // 未設定のものが一つもなければ条件成立
            falseComponentIndexList.Clear();
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