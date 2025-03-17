using NUnit.Framework;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Editor.Fact
{
    public class ExecutedElementUtilityTest
    {
        [Test]
        public void IsJumpSequence()
        {
            // 2/3番目にアクセルが含まれているジャンプはジャンプ・シークェンス
            Assert.IsTrue(ExecutedElementUtility.IsJumpSequence("3Lze!+2A"));
            Assert.IsTrue(ExecutedElementUtility.IsJumpSequence("3Lz<<+2A"));
            Assert.IsTrue(ExecutedElementUtility.IsJumpSequence("3Lz+2A+2T"));
            Assert.IsTrue(ExecutedElementUtility.IsJumpSequence("3Lz+2A+2A"));
            // オイラー直後のアクセルはジャンプ・コンビネーション
            Assert.IsFalse(ExecutedElementUtility.IsJumpSequence("3Lz+1Eu+2A"));
            // これら以外の連続ジャンプはジャンプ・コンビネーション
            Assert.IsFalse(ExecutedElementUtility.IsJumpSequence("3Lz+2Lo+2T"));
            Assert.IsFalse(ExecutedElementUtility.IsJumpSequence("3Lz+1Eu+2T"));
        }
    }
}
