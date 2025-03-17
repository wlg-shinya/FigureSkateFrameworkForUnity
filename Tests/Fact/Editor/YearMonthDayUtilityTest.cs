using NUnit.Framework;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Editor.Fact
{
    public class YearMonthDayUtilityTest
    {
        [Test]
        public void GetAge()
        {
            Assert.AreEqual(YearMonthDayUtility.GetAge(new(2024, 7, 1), new(1982, 6, 1)), 42);
            Assert.AreEqual(YearMonthDayUtility.GetAge(new(2024, 7, 1), new(1982, 8, 2)), 41);
        }

        [Test]
        public void GetSkateYearString()
        {
            Assert.AreEqual(YearMonthDayUtility.GetSkateYearString(new(2024, 6, 30)), "2023-24");
            Assert.AreEqual(YearMonthDayUtility.GetSkateYearString(new(2024, 7, 1)), "2024-25");
            Assert.AreEqual(YearMonthDayUtility.GetSkateYearString(new(1982, 6, 1)), "1981-82");
        }
    }
}
