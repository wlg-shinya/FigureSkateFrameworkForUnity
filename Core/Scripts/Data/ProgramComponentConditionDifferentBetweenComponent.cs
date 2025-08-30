using System;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で同じ要素を設定していないか判断する。ただし同構成中の同じ要素は許容
    [Serializable]
    public class ProgramComponentConditionDifferentBetweenComponent : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;

        public override bool Condition(ProgramComponent[] components)
        {
            // リストを初期化
            falseComponentIndexList.Clear();

            // 1. 対象となるコンポーネントを、元のインデックスと共に抽出
            var targetComponentsWithIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .ToList();

            // 2. 各コンポーネントから要素IDを「元のインデックス」とペアにして平坦化(flatten)する
            //    この時、コンポーネント"内"の重複は許容されるため、先にDistinct()でユニークにする
            var allElementsWithIndices = targetComponentsWithIndices
                .SelectMany(compWithIndex => compWithIndex.component.elementIds
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Distinct()
                    .Select(id => new { ElementId = id, OriginalIndex = compWithIndex.index }));

            // 3. 要素IDでグループ化し、複数のコンポーネントで使われているもの（＝重複）を特定
            var violatingIndices = allElementsWithIndices
                .GroupBy(x => x.ElementId) // 要素IDでグループ化
                .Where(g => g.Count() > 1)   // 2つ以上のコンポーネントで使われているグループに絞る
                .SelectMany(g => g.Select(item => item.OriginalIndex)) // そのグループに含まれる元のインデックスをすべて取得
                .Distinct();                 // インデックスの重複を削除

            // 4. 違反したコンポーネントのインデックスをリストに追加
            falseComponentIndexList.AddRange(violatingIndices);

            // 違反リストが空であれば条件成立
            return falseComponentIndexList.Count == 0;
        }
    }
}