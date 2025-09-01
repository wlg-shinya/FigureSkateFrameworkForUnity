using System;
using System.Collections.Generic;
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

        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            // 1. このルールのチェック対象となるコンポーネントのインデックスをすべて探し出す
            var targetIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .Select(x => x.index)
                .ToList();

            // 2. チェック対象のコンポーネントの中に、パターンに一致する要素が1つでも存在するか調べる
            bool isPatternFound = targetIndices.Any(index =>
                components[index].elementIds.Any(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, pattern))
            );

            // 3. パターンが見つかれば条件成功（エラーリストは空）、見つからなければ失敗（エラーリストは対象インデックス全て）
            falseComponentIndexList = isPatternFound ? new List<int>() : targetIndices;

            return isPatternFound;
        }
    }
}