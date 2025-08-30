using System;
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

        public override bool Condition(ProgramComponent[] components)
        {
            // リストを初期化
            falseComponentIndexList.Clear();

            // チェック対象となるコンポーネントとその元のインデックスを抽出
            var targetComponentsWithIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .ToList();

            // マッチングに使用するため、コンポーネント部分だけの可変リストを作成
            // （一度マッチしたコンポーネントは次の検索から除外するため）
            var availableComponents = targetComponentsWithIndices.Select(x => x.component).ToList();

            int matchesFound = 0;

            // 各パターンに対して、まだ使われていないコンポーネントから一致するものを探す
            foreach (var pattern in patterns)
            {
                // パターンに一致する最初の要素を持つコンポーネントを探す
                var matchedComponent = availableComponents.FirstOrDefault(comp =>
                    !string.IsNullOrEmpty(comp.elementIds.FirstOrDefault()) && Regex.IsMatch(comp.elementIds[0], pattern));

                if (matchedComponent != null)
                {
                    matchesFound++;
                    // 一致したコンポーネントは次の検索から除外する
                    availableComponents.Remove(matchedComponent);
                }
            }

            // 全てのパターンに一致するものが見つかったかどうかで成否を判断
            bool isConditionMet = matchesFound == patterns.Length;

            // 条件を満たしていない場合、対象となったコンポーネントのインデックスをすべてリストに追加
            if (!isConditionMet)
            {
                foreach (var item in targetComponentsWithIndices)
                {
                    falseComponentIndexList.Add(item.index);
                }
            }

            // 条件を満たしていればリストは空、満たしていなければリストにインデックスが入っている
            return falseComponentIndexList.Count == 0;
        }
    }
}