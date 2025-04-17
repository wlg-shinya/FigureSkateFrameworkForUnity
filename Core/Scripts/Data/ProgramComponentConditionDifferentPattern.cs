using System;
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

        public override bool Condition(ProgramComponent[] components)
        {
            // 指定した構成グループのデータを抽出
            var groups = components.Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId))).ToArray();

            // 指定した構成グループ間で同じパターンの要素を設定していないか判断する
            bool SearchPatterm(ProgramComponent[] groups, string[] patterns)
            {
                // グループの先頭が指定パターンのどれとマッチしているか探す
                var foundPattern = patterns
                    .ToList()
                    .Find(pattern => groups[0].elementIds
                        .Where(placedElementId => !string.IsNullOrEmpty(placedElementId))
                        .Any(placedElementId => Regex.IsMatch(placedElementId, pattern))
                    );
                if (!string.IsNullOrEmpty(foundPattern))
                {
                    if (groups.Length > 1)
                    {
                        // 今回扱ったグループの一員と見つかったパターンを除外して再度検索
                        var newGroups = groups.Where((x, i) => i != 0).ToArray();
                        var newPatterns = patterns.Where(x => !Equals(x, foundPattern)).ToArray();
                        return SearchPatterm(newGroups, newPatterns);
                    }
                    else
                    {
                        // グループの最後だった場合、すべてのパターンを見つけられたので同じパターンを指定していないことが確定
                        return true;
                    }
                }
                else
                {
                    if (groups[0].elementIds.Any(x => string.IsNullOrEmpty(x)))
                    {
                        // まだ設定が不完全の構成なのでエラーとしない
                        return true;
                    }
                    else
                    {
                        // 同じパターンかパターン外の要素を指定しているのでエラー
                        return false;
                    }
                }
            }
            return SearchPatterm(groups, patterns);
        }
    }
}