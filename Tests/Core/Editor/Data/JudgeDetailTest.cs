using System.Collections.Generic;
using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Editor.Core.Data
{
    public class JudgeDetailTest
    {
        [Test]
        public void TesInfoMark()
        {
            JudgeDetail judgeDetail = new(2);

            // データが空なので情報記号は空文字
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "");
            // 意図通り付与される
            var marks0 = new string[] { "q" };
            var infoMarks0 = new JudgeDetail.Tes.InfoMarks()
            {
                marks = new(marks0)
            };
            judgeDetail.tes[0].infoMarksList.Add(infoMarks0);
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "q");
            // 国際基準に合わせた順序で付与される
            judgeDetail.tes[0].infoMarksList[0].marks.Add("!");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "!q");
            // 実際はジャンプ系記号(!qなど)とスピン系記号(V)は混在しないが仕組みのうえでは混在は許容している
            judgeDetail.tes[0].infoMarksList[0].marks.Add("V");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "!qV");
            // より重い罰則の記号のみが付与される
            judgeDetail.tes[0].infoMarksList[0].marks.Add("e");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "eqV");
            judgeDetail.tes[0].infoMarksList[0].marks.Add("<");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "e<V");
            judgeDetail.tes[0].infoMarksList[0].marks.Add("<<");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "e<<V");
            // 同じ記号が付与されても重複しない
            judgeDetail.tes[0].infoMarksList[0].marks.Add("e");
            judgeDetail.tes[0].infoMarksList[0].marks.Add("<<");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "e<<V");
            // コンビネーション中に新たな記号が付与されても一つ目の影響は受けない
            var marks1 = new string[] { "!", "q" };
            var infoMarks1 = new JudgeDetail.Tes.InfoMarks()
            {
                marks = new List<string>(marks1)
            };
            judgeDetail.tes[1].infoMarksList.Add(infoMarks1);
            Assert.AreEqual(judgeDetail.tes[1].InfoMark(), "!q");
            Assert.AreEqual(judgeDetail.tes[0].InfoMark(), "e<<V");
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
