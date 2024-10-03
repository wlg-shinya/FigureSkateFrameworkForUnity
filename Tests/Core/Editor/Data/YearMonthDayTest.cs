using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Editor.Core.Data
{
    public class YearMonthDayTest
    {
        [Test]
        public void Equals()
        {
            var ymd1 = new YearMonthDay(2024, 12, 31);
            var ymd2 = new YearMonthDay(2024, 12, 31);
            Assert.AreEqual(ymd1, ymd2);
            Assert.IsTrue(ymd1.Equals(ymd2));
            Assert.AreEqual(ymd1.GetHashCode(), ymd2.GetHashCode());
            Assert.IsTrue(ymd1 == ymd2);
            Assert.IsFalse(ymd1 != ymd2);
            var ymd3 = new YearMonthDay(2024, 12, 30);
            Assert.IsTrue(ymd1 != ymd3);
            Assert.IsFalse(ymd1 == ymd3);
        }

        [Test]
        public void Compare()
        {
            var ymd1 = new YearMonthDay(2024, 12, 31);
            var ymd2 = new YearMonthDay(2024, 12, 31);
            Assert.IsTrue(ymd1 >= ymd2);
            Assert.IsTrue(ymd1 <= ymd2);
            Assert.IsTrue(ymd2 <= ymd1);
            Assert.IsTrue(ymd2 >= ymd1);
            var ymd3 = new YearMonthDay(2024, 12, 30);
            Assert.IsTrue(ymd1 > ymd3);
            Assert.IsFalse(ymd1 < ymd3);
            Assert.IsTrue(ymd3 < ymd1);
            Assert.IsFalse(ymd3 > ymd1);
            Assert.IsTrue(ymd1 >= ymd3);
            Assert.IsFalse(ymd1 <= ymd3);
            Assert.IsTrue(ymd3 <= ymd1);
            Assert.IsFalse(ymd3 >= ymd1);
            var ymd4 = new YearMonthDay(2024, 11, 31);
            Assert.IsTrue(ymd1 > ymd4);
            Assert.IsFalse(ymd1 < ymd4);
            Assert.IsTrue(ymd4 < ymd1);
            Assert.IsFalse(ymd4 > ymd1);
            Assert.IsTrue(ymd1 >= ymd4);
            Assert.IsFalse(ymd1 <= ymd4);
            Assert.IsTrue(ymd4 <= ymd1);
            Assert.IsFalse(ymd4 >= ymd1);
            var ymd5 = new YearMonthDay(2023, 12, 31);
            Assert.IsTrue(ymd1 > ymd5);
            Assert.IsFalse(ymd1 < ymd5);
            Assert.IsTrue(ymd5 < ymd1);
            Assert.IsFalse(ymd5 > ymd1);
            Assert.IsTrue(ymd1 >= ymd5);
            Assert.IsFalse(ymd1 <= ymd5);
            Assert.IsTrue(ymd5 <= ymd1);
            Assert.IsFalse(ymd5 >= ymd1);
            var ymd6 = new YearMonthDay(2023, 9, 13);
            var ymd7 = new YearMonthDay(2023, 10, 7);
            Assert.IsTrue(ymd6 < ymd7);
            Assert.IsFalse(ymd6 > ymd7);
            Assert.IsTrue(ymd6 <= ymd7);
            Assert.IsFalse(ymd6 >= ymd7);
        }
    }
}
