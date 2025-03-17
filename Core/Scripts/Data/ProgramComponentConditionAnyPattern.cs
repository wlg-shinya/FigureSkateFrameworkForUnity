using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で指定パターンの要素がひとつでも設定されているか判断する
    [Serializable]
    public class ProgramComponentConditionAnyPattern : ProgramComponentCondition
    {
        // 対象の構成要素枠のID群
        public string[] elementPlaceableSetIds;
        // 指定パターン
        public string pattern;

        public override bool Condition(ProgramComponent[] components)
        {
            return components
                // 対象の構成要素枠だけに絞る
                .Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId)))
                // 構成されている要素が指定パターンと一致しているものがあるかどうか判断する
                .Any(group => group.elementIds.Any(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, pattern)));
        }
    }
}