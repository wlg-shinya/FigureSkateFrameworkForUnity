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
            // 指定した構成グループの全配置済み構成要素を抽出
            var allPlacedElementIds = components
                .Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId)))
                .SelectMany(x => x.elementIds)
                .Where(x => !string.IsNullOrEmpty(x));
            // 一意の構成要素も用意する
            var uniquePlacedElementIds = allPlacedElementIds.Distinct();

            // 同じ要素が何回設定されているか一覧の作成
            var countList = uniquePlacedElementIds.Select(elementId => allPlacedElementIds.Count(elementId2 => Equals(elementId, elementId2)));

            // 一つでも指定数を超えていたらエラー
            return !countList.Any(x => x > count);
        }
    }
}