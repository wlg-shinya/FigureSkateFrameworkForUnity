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
            var notPlacedElementIds = components
                .SelectMany(x => x.elementIds)
                .Where(x => string.IsNullOrEmpty(x));
            return notPlacedElementIds.Count() == 0;
        }
    }
}