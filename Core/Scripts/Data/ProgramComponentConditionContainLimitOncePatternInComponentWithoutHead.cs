using System;
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

        public override bool Condition(ProgramComponent[] components)
        {
            // 対象となる候補の構成データを抽出
            var candidates = components.Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId)));

            // 構成の先頭以外に指定パターンが含まれるのが上限ひとつまでか判定
            {
                var containPatternComponent = candidates
                    // 一つしかない構成は対象外
                    .Where(x => x.elementIds.Count() >= 2)
                    // 先頭要素以外の構成に指定パターンが含んでいるものだけ抽出
                    .Where(x => x.elementIds
                        // 未設定の構成要素は無視
                        .Where(x => !string.IsNullOrEmpty(x))
                        // 先頭要素を除外した構成要素IDに絞る
                        .Where((x, i) => i > 0)
                        // 構成要素IDの中に指定パターンが含んでいるものがあるかどうか判断
                        .Any(elementId => Regex.IsMatch(elementId, pattern))
                        );

                // パターンを含む構成要素がひとつ以下なら条件成立
                return containPatternComponent.Count() <= 1;
            }
        }
    }
}