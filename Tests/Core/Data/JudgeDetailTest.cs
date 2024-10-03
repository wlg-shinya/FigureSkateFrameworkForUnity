using System.Collections.Generic;
using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Core.Data
{
    public class JudgeDetailTest
    {
        [Test]
        public void TesInfoMark()
        {
            JudgeDetail judgeDetail = new(2);

            // データが空なので情報記号は空文字
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "");
            // 記号は登録された順に結合される
            var marks = new string[] { "q", "<", "!", "e", "V" };
            var infoMarks = new JudgeDetail.Tes.InfoMarks()
            {
                marks = new List<string>(marks)
            };
            judgeDetail.tes[0].infoMarksList.Add(infoMarks);
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "q<!eV");
            // コンビネーションの各要素で同じ記号が付与されても重複しない
            judgeDetail.tes[0].infoMarksList.Add(infoMarks);
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "q<!eV");
            // コンビネーションの各要素で < と << が別々に付与された場合は << しか表示されない
            var urMarks = new string[] { "<" };
            var dgMarks = new string[] { "<<" };
            var urInfoMarks = new JudgeDetail.Tes.InfoMarks()
            {
                marks = new List<string>(urMarks)
            };
            var dgInfoMarks = new JudgeDetail.Tes.InfoMarks()
            {
                marks = new List<string>(dgMarks)
            };
            judgeDetail.tes[1].infoMarksList.Add(urInfoMarks);
            judgeDetail.tes[1].infoMarksList.Add(dgInfoMarks);
            Assert.AreEqual(judgeDetail.tes[1].InfoMark(), "<<");
        }

        [Test]
        public void TesRefereeGoeAverage()
        {
            JudgeDetail judgeDetail = new(1);

            // データが空なので審判平均値は0
            Assert.AreApproximatelyEqual(judgeDetail.tes[0].RefereeGoeAverage(), 0.0f);
            // 最大値と最小値を省いた平均値になるので先頭二つは無視されて1
            judgeDetail.tes[0].refereeGoe = new int[] { 5, -3, 1, 1, 1, 1, 1, 1, 1 };
            Assert.AreApproximatelyEqual(judgeDetail.tes[0].RefereeGoeAverage(), 1.0f);
            // 最大値と最小値が複数あった場合は一つだけ採用されるので先頭2要素以外の平均値になって1
            judgeDetail.tes[0].refereeGoe = new int[] { 5, -4, 5, -4, 5, -4, 5, -4, 4 };
            Assert.AreApproximatelyEqual(judgeDetail.tes[0].RefereeGoeAverage(), 1.0f);
        }

        [Test]
        public void PcsRefereeScoreAverage()
        {
            JudgeDetail judgeDetail = new(1);

            // データが空なので審判平均値は0
            Assert.AreApproximatelyEqual(judgeDetail.pcs[0].RefereeScoreAverage(), 0.0f);
            // 最大値と最小値を省いた平均値になるので先頭二つは無視されて1.0f
            judgeDetail.pcs[0].refereeScore = new float[] { 10.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };
            Assert.AreApproximatelyEqual(judgeDetail.pcs[0].RefereeScoreAverage(), 1.0f);
            // 最大値と最小値が複数あった場合は一つだけ採用されるので先頭2要素以外の平均値になって5.0f
            judgeDetail.pcs[0].refereeScore = new float[] { 10.0f, 1.0f, 10.0f, 1.0f, 10.0f, 1.0f, 10.0f, 1.0f, 2.0f };
            Assert.AreApproximatelyEqual(judgeDetail.pcs[0].RefereeScoreAverage(), 5.0f);
        }
    }
}
