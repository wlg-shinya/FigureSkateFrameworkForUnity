using System;
using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Core
{
    // 2連続コンビネーションの配置済み構成要素が不正な組み合わせでないか判断する
    [Serializable]
    public class ElementPlaceableSetConditionInvalid2Combination : ElementPlaceableSetCondition
    {
        public string firstInvalidElementIdPattern;
        public string secondInvalidElementIdPattern;

        public override bool Condition(string[] placedElementIds)
        {
            if (placedElementIds.Length == 2)
            {
                return !(Regex.IsMatch(placedElementIds[0], firstInvalidElementIdPattern) && Regex.IsMatch(placedElementIds[1], secondInvalidElementIdPattern));
            }
            else
            {
                // 特にチェックする必要のない組み合わせ
                return true;
            }
        }
    }
}