using System;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Fact.Data
{
    // いち選手がプログラムで判定されたスコアデータ
    // これを UI.JudgesScore に渡すことで内容を表示してくれる
    [Serializable]
    public class JudgesScore
    {
        // 判定したプログラム名
        public string programName;
        // 選手のスコア上の名前
        public string nameOnScore;
        // 選手の国籍
        public string nation;
        // 実行順番
        public int startingNumber;
        // 何位だったか
        public int rank;
        // 判定の詳細データ
        public JudgeDetail judgeDetail;
    }
}