using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 指定した構成グループ間で指定パターンの要素がひとつでも設定されているか判断する
    [Serializable]
    public class ProgramComponentConditionAnyPattern : ProgramComponentCondition
    {
        // 対象の構成要素枠のID群
        public string[] elementPlaceableSetIds;
        // 指定パターン
        public string pattern;

        public override bool Condition(ProgramComponent[] components)
        {
            // リストを初期化
            falseComponentIndexList.Clear();

            // 対象の構成要素枠だけに絞って、その中で指定パターンに一致する要素があるかチェック
            bool isConditionMet = components
                .Where(c => elementPlaceableSetIds.Contains(c.elementPlaceableSetId))
                .Any(c => c.elementIds.Any(id => !string.IsNullOrEmpty(id) && Regex.IsMatch(id, pattern)));

            // 条件を満たしていない場合（＝一致する要素が一つもなかった場合）
            if (!isConditionMet)
            {
                // そもそもチェック対象だったコンポーネントのインデックスをすべてリストアップする
                for (int i = 0; i < components.Length; i++)
                {
                    if (elementPlaceableSetIds.Contains(components[i].elementPlaceableSetId))
                    {
                        falseComponentIndexList.Add(i);
                    }
                }
            }

            // 条件を満たしていればリストは空、満たしていなければリストにインデックスが入っている
            return falseComponentIndexList.Count == 0;
        }
    }
}