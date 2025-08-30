using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定パターン構成要素の中で同じ要素を指定数以内の構成で抑えているかを複数の条件の重ね合わせで判断する
    [Serializable]
    public class ProgramComponentConditionWithinInPatternMultipleConditions : ProgramComponentCondition
    {
        [Serializable]
        public class Data
        {
            // このデータが無効か
            public bool disable;
            // グルーピングする構成要素の指定パターン
            public string groupPattern;
            // 同じ構成要素の最大同時構成数
            public int maxPlacedCount;
            // 同じ構成要素の最大同時構成の許容数
            public int maxPlacedAllowCount;

            public List<int> GetViolatingIndices(ProgramComponent[] components)
            {
                var violatingIndices = new HashSet<int>();

                // 1. パターンに一致する全要素を、元のコンポーネントのインデックス付きで抽出
                var targetElementsWithIndices = components
                    .Select((component, index) => new { component, index })
                    .SelectMany(c => c.component.elementIds
                        .Where(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, groupPattern))
                        .Select(id => new { ElementId = id, OriginalIndex = c.index }))
                    .ToList();

                // 要素IDでグループ化して集計
                var groupedElements = targetElementsWithIndices.GroupBy(x => x.ElementId).ToList();

                // 2.【違反チェック1】最大同時構成数(maxPlacedCount)を超えている要素がないか
                var exceedingCountGroups = groupedElements.Where(g => g.Count() > maxPlacedCount);
                foreach (var group in exceedingCountGroups)
                {
                    foreach (var item in group) violatingIndices.Add(item.OriginalIndex);
                }

                // 3.【違反チェック2】最大同時構成数に達している要素の数が許容数(maxPlacedAllowCount)を超えていないか
                var groupsAtMaxCount = groupedElements.Where(g => g.Count() == maxPlacedCount).ToList();
                if (groupsAtMaxCount.Count > maxPlacedAllowCount)
                {
                    // 違反している場合、その原因となった要素を持つコンポーネントをすべてリストアップ
                    foreach (var group in groupsAtMaxCount)
                    {
                        foreach (var item in group) violatingIndices.Add(item.OriginalIndex);
                    }
                }

                return violatingIndices.ToList();
            }
        }
        [Serializable]
        public class ConditionData
        {
            public Data current;
            public Data ifTrue;
            public Data ifFalse;
            public List<int> GetViolatingIndices(ProgramComponent[] components)
            {
                if (current.disable)
                {
                    return new List<int>(); // currentが無効なら常に成功
                }

                var currentViolations = current.GetViolatingIndices(components);

                // currentが条件を満たした場合 (違反なし)
                if (currentViolations.Count == 0)
                {
                    // ifTrueが無効なら成功、そうでなければifTrueの結果を返す
                    return ifTrue.disable ? new List<int>() : ifTrue.GetViolatingIndices(components);
                }
                // currentが条件を満たさなかった場合 (違反あり)
                else
                {
                    // ifFalseが無効な場合、currentの失敗がそのまま最終的な失敗となる
                    if (ifFalse.disable)
                    {
                        return currentViolations;
                    }
                    // そうでなければifFalseの結果を返す
                    return ifFalse.GetViolatingIndices(components);
                }
            }
        }
        public ConditionData[] conditionDatas;

        public override bool Condition(ProgramComponent[] components)
        {
            falseComponentIndexList.Clear();
            var allViolatingIndices = new HashSet<int>();

            // 全ての条件セットをチェックし、違反インデックスを集約する
            foreach (var data in conditionDatas)
            {
                var violations = data.GetViolatingIndices(components);
                foreach (var index in violations)
                {
                    allViolatingIndices.Add(index);
                }
            }

            if (allViolatingIndices.Any())
            {
                falseComponentIndexList.AddRange(allViolatingIndices);
            }

            return !falseComponentIndexList.Any();
        }
    }
}