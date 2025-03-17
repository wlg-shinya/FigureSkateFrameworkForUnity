using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 3連続コンビネーションの3番目の要素の制限に引っかかっていないか判断する
    // ただし2番目の構成次第では3番目の制限は解除される
    [Serializable]
    public class ElementPlaceableSetConditionLimit3CombinationThird : ElementPlaceableSetCondition
    {
        public string[] allowElementIdPatterns;
        public string limitOffSecondElementIdPattern;

        public override bool Condition(string[] placedElementIds)
        {
            if (placedElementIds.Length == 3)
            {
                // 3番目が未設定か2番目の構成要素が制限解除な要素だったら制限なし
                if (Equals(placedElementIds[2], "") || Regex.IsMatch(placedElementIds[1], limitOffSecondElementIdPattern))
                {
                    return true;
                }
                else
                {
                    // 制限が有効なので指定した許可パターンの構成要素を含んでいない場合はエラー
                    return allowElementIdPatterns.Any(x => Regex.IsMatch(placedElementIds[2], x));
                }
            }
            else
            {
                // 特にチェックする必要のない組み合わせ
                return true;
            }
        }
    }
}