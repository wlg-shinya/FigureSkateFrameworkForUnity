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
            // リストを初期化
            falseComponentIndexList.Clear();

            // 条件違反の可能性があるコンポーネント（＝パターンを含むもの）のインデックスを探す
            var componentsWithPatternIndices = components
                .Select((component, index) => new { component, index })
                // 1. 対象の構成要素グループに絞る
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                // 2. 構成要素が2つ以上あるものに絞る
                .Where(x => x.component.elementIds.Count() >= 2)
                // 3. 先頭以外の要素に指定パターンが含まれているかチェック
                .Where(x => x.component.elementIds
                    .Skip(1) // 先頭要素をスキップ
                    .Any(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, pattern)))
                .Select(x => x.index)
                .ToList();

            // パターンを含むコンポーネントが2つ以上ある場合、条件違反
            if (componentsWithPatternIndices.Count > 1)
            {
                // 違反したすべてのコンポーネントのインデックスをリストに追加
                falseComponentIndexList.AddRange(componentsWithPatternIndices);
            }

            // 条件を満たしていればリストは空、満たしていなければリストにインデックスが入っている
            return falseComponentIndexList.Count == 0;
        }
    }
}