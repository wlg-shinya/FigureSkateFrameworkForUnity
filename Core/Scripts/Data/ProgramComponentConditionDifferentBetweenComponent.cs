using System;
using System.Collections.Generic;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で同じ要素を設定していないか判断する。ただし同構成中の同じ要素は許容
    [Serializable]
    public class ProgramComponentConditionDifferentBetweenComponent : ProgramComponentCondition
    {
        // 対象の構成要素ID群
        public string[] elementPlaceableSetIds;

        public override bool Condition(ProgramComponent[] components, out List<int> falseComponentIndexList)
        {
            // 1. 対象コンポーネントから、コンポーネント内でユニークな要素IDを「元のインデックス」とペアにして平坦化する
            var allUniqueElementsWithIndices = components
                .Select((component, index) => new { component, index })
                .Where(x => elementPlaceableSetIds.Contains(x.component.elementPlaceableSetId))
                .SelectMany(compWithIndex => compWithIndex.component.elementIds
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Distinct() // コンポーネント内の重複は許容するため、ここでユニークにする
                    .Select(id => new { ElementId = id, OriginalIndex = compWithIndex.index }));

            // 2. 要素IDでグループ化し、複数のコンポーネントで使用されているID（＝ルール違反）を探す
            //    そして、それらに関わる全てのコンポーネントインデックスをリストアップする
            var violatingIndices = allUniqueElementsWithIndices
                .GroupBy(x => x.ElementId)   // 要素IDでグループ化
                .Where(g => g.Count() > 1)     // 2つ以上のコンポーネントで使われているグループに絞る
                .SelectMany(g => g.Select(item => item.OriginalIndex)) // そのグループに含まれる元のインデックスをすべて取得
                .Distinct()                    // インデックスの重複を削除
                .ToList();

            // 3. 違反したインデックスのリストをoutパラメータに設定
            falseComponentIndexList = violatingIndices;

            // 4. 違反リストが空であれば条件成立
            return falseComponentIndexList.Count == 0;
        }
    }
}