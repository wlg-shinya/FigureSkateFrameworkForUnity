using System.Text.RegularExpressions;

namespace Wlg.FigureSkate.Fact.Utility
{
    public static class ExecutedElementUtility
    {
        // 実行結果構成要素がジャンプ・シークェンスかどうか
        public static bool IsJumpSequence(string executedElement)
        {
            return Regex.IsMatch(executedElement, @"[^\+1Eu.*]\+\dA");
        }
    }
}
