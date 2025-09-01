using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間にて指定したパターンを先頭以外に含む構成が上限ひとつまでか判断する
    [Serializable]
    public class ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;
        // 先頭以外に含むのがひとつだけであるべきパターン
        public string pattern;

        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            // 1. ルールに違反している可能性のあるコンポーネント
            //    (＝指定パターンを先頭以外に含むもの) のインデックスをすべて探し出す
            var violatingIndices = components
                .Select((component, index) => new { component, index })
                .Where(x =>
                    // 対象の構成要素グループであり、
                    elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId) &&
                    // 構成要素が2つ以上あり、
                    x.component.elementIds.Length >= 2 &&
                    // 先頭以外の要素に指定パターンが含まれている
                    x.component.elementIds.Skip(1).Any(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, pattern)))
                .Select(x => x.index)
                .ToList();

            // 2. パターンを含むコンポーネントが2つ以上あればルール違反
            bool isConditionMet = violatingIndices.Count <= 1;

            // 3. 条件を満たせばエラーリストは空、違反すれば違反したインデックスのリストを返す
            falseComponentIndexList = isConditionMet ? new List<int>() : violatingIndices;

            return isConditionMet;
        }
    }
}