using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループの第1構成要素で指定パターンの要素をすべて設定しているか判断する
    [Serializable]
    public class ProgramComponentConditionContainAllPatternInFirst : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;
        // 含めるべきパターン
        public string[] patterns;

        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            // 1. このルールのチェック対象となるコンポーネントとその元のインデックスを抽出
            var targetComponentsWithIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .ToList();

            // 2. マッチングに使用するため、対象コンポーネントの「第1要素のID」の可変リストを作成
            //    (一度マッチしたIDは次の検索から除外するため)
            var availableFirstElementIds = targetComponentsWithIndices
                .Select(x => x.component.elementIds.FirstOrDefault())
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            int matchesFound = 0;

            // 3. 各パターンに対して、まだ使われていない要素IDから一致するものを探す
            foreach (var pattern in patterns)
            {
                var matchedId = availableFirstElementIds.FirstOrDefault(id => Regex.IsMatch(id, pattern));

                if (matchedId != null)
                {
                    matchesFound++;
                    // 一致したIDは次の検索から除外する (一対一マッチングのため)
                    availableFirstElementIds.Remove(matchedId);
                }
            }

            // 4. 全てのパターンに一致するものが見つかったかどうかで成否を判断
            bool isConditionMet = matchesFound == patterns.Length;

            if (isConditionMet)
            {
                // 条件を満たした場合、エラーリストは空
                falseComponentIndexList = new List<int>();
            }
            else
            {
                // 条件を満たさなかった場合、対象となったコンポーネントのインデックスをすべてエラーとする
                falseComponentIndexList = targetComponentsWithIndices.Select(x => x.index).ToList();
            }

            return isConditionMet;
        }
    }
}