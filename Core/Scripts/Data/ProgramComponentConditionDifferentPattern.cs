using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で同じパターンの要素を設定していないか判断する
    [Serializable]
    public class ProgramComponentConditionDifferentPattern : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;
        // 重複してはいけないパターン
        public string[] patterns;

        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            // 1. 対象となるコンポーネントを元のインデックスと共に抽出
            var targetComponentsWithIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .ToList();

            // 2. 未設定の要素を持つコンポーネントが対象内にあれば、チェックせず常に成功とする
            if (targetComponentsWithIndices.Any(c => c.component.elementIds.Any(string.IsNullOrEmpty)))
            {
                falseComponentIndexList = new List<int>();
                return true;
            }

            // 違反したコンポー-ネントのインデックスを重複なく保持するためのHashSet
            var violatingIndices = new HashSet<int>();

            // 3.【重複チェック】同じパターンを複数のコンポーネントが使用していないか
            foreach (var pattern in patterns)
            {
                var matchingComponents = targetComponentsWithIndices
                    .Where(c => c.component.elementIds.Any(id => Regex.IsMatch(id, pattern)))
                    .ToList();

                // パターンに一致するコンポーネントが2つ以上あれば、それは重複であり条件違反
                if (matchingComponents.Count > 1)
                {
                    // 違反した（重複の原因となった）コンポーネントのインデックスをすべて追加
                    foreach (var comp in matchingComponents)
                    {
                        violatingIndices.Add(comp.index);
                    }
                }
            }

            // 4.【パターン網羅チェック】いずれのパターンにも一致しない要素が設定されていないか
            var componentsWithoutMatch = targetComponentsWithIndices
                .Where(c => !patterns.Any(p => c.component.elementIds.Any(id => Regex.IsMatch(id, p))));

            foreach (var comp in componentsWithoutMatch)
            {
                violatingIndices.Add(comp.index);
            }

            // 5. 違反したインデックスを最終的なリストに変換
            falseComponentIndexList = violatingIndices.ToList();

            // 6. 違反リストが空であれば条件成立
            return falseComponentIndexList.Count == 0;
        }
    }
}