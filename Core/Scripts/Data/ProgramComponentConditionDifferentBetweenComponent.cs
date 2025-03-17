using System;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で同じ要素を設定していないか判断する。ただし同構成中の同じ要素は許容
    [Serializable]
    public class ProgramComponentConditionDifferentBetweenComponent : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;

        public override bool Condition(ProgramComponent[] components)
        {
            // 指定した構成グループのデータを抽出
            var groups = components.Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId)));

            // ほかの構成の中に自身に設定されている構成要素がひとつでも含まれていたらfalse
            return !groups.Any(group1 =>
            {
                return group1.elementIds.Any(elementId =>
                {
                    if (!string.IsNullOrEmpty(elementId))
                    {
                        return groups.Any(group2 =>
                        {
                            if (!ReferenceEquals(group1, group2))
                            {
                                return group2.elementIds.Any(x => Equals(x, elementId));
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                    else
                    {
                        return false;
                    }
                });
            });
        }
    }
}