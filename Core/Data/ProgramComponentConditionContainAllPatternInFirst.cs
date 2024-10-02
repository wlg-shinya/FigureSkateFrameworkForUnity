using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core.Data
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
            // 指定した構成グループのデータを抽出
            var groups = components.Where(argument => elementPlaceableSetIds.Any(id => Equals(id, argument.elementPlaceableSetId))).ToArray();

            // 指定した構成グループ間でパターンの要素を設定しているか判断する
            bool SearchPatterm(ProgramComponent[] groups, string[] patterns)
            {
                // すべてのパターンを見つけられたので条件成立
                if (patterns.Length == 0)
                {
                    return true;
                }

                // 各グループの先頭構成要素が指定パターンとマッチしているか探す
                ProgramComponent matchedGroup = null;
                var matchedPattern = patterns
                    .ToList()
                    .Find(pattern => groups
                        .Any(group =>
                        {
                            if (!string.IsNullOrEmpty(group.elementIds[0]) && Regex.IsMatch(group.elementIds[0], pattern))
                            {
                                matchedGroup = group;
                                return true;
                            }
                            else return false;
                        })
                    );
                if (!string.IsNullOrEmpty(matchedPattern) && matchedGroup != null)
                {
                    // 今回扱ったグループの一員と見つかったパターンを除外して再度検索
                    var newGroups = groups.Where((x) => !ReferenceEquals(x, matchedGroup)).ToArray();
                    var newPatterns = patterns.Where(x => !ReferenceEquals(x, matchedPattern)).ToArray();
                    return SearchPatterm(newGroups, newPatterns);
                }
                else
                {
                    // パターンをすべて見つけられなかったので設定漏れがある。条件不成立
                    return false;
                }
            }
            return SearchPatterm(groups, patterns);
        }
    }
}