using System;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で同じ要素を指定数以内の構成で抑えているか判断する
    [Serializable]
    public class ProgramComponentConditionWithin : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;
        // 重複許容数
        public int count;

        public override bool Condition(ProgramComponent[] components)
        {
            // リストを初期化
            falseComponentIndexList.Clear();

            // 1. 対象グループの全要素を、それが含まれていたコンポーネントの元のインデックスとペアにして平坦化する
            var allElementsWithIndices = components
                .Select((component, index) => new { component, index })
                // 対象グループに絞り込み
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                // 各コンポーネントの要素を「元のインデックス」とペアにして平坦化
                .SelectMany(compWithIndex => compWithIndex.component.elementIds
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Select(id => new { ElementId = id, OriginalIndex = compWithIndex.index }));

            // 2. 要素IDでグループ化し、許容数(count)を超えて使用されている要素を特定する
            var violatingIndices = allElementsWithIndices
                .GroupBy(x => x.ElementId)      // 要素IDでグループ化
                .Where(g => g.Count() > count)  // 許容数を超えているグループ(要素)に絞る
                .SelectMany(g => g.Select(item => item.OriginalIndex)) // そのグループに含まれる元のインデックスをすべて取得
                .Distinct();                    // インデックスが重複しないようにユニークにする

            // 3. 違反したコンポーネントのインデックスをリストに追加
            falseComponentIndexList.AddRange(violatingIndices);

            // 違反リストが空であれば条件成立
            return falseComponentIndexList.Count == 0;
        }
    }
}