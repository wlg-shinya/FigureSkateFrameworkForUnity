using NUnit.Framework;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Wlg.FigureSkate.Core;
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

        [Test]
        public void Constructor()
        {
            try
            {
                // 意図通りの指定
                var ymd = new YearMonthDay("2025/1/27");
            }
            catch
            {
                Assert.IsTrue(false); // ここには到達しないはず
            }
            try
            {
                // 意図しないの指定 - /を使用していない
                var ymd = new YearMonthDay("2025,1,27");
                Assert.IsTrue(false); // ここには到達しないはず
            }
            catch
            {
            }
            try
            {
                // 意図しないの指定 - /がたりない
                var ymd = new YearMonthDay("2025/127");
                Assert.IsTrue(false); // ここには到達しないはず
            }
            catch
            {
            }
            try
            {
                // 意図しないの指定 - /が多い
                var ymd = new YearMonthDay("2025/1/2/7");
                Assert.IsTrue(false); // ここには到達しないはず
            }
            catch
            {
            }
        }

        [Test]
        public void Localize()
        {
            var ymd = new YearMonthDay("2025/1/23");

            // jaは年/月/日
            LocalizationSettings.SelectedLocale = Locale.CreateLocale("ja");
            Assert.AreEqual(ymd.ToString(), "2025/1/23");

            // enは月/日/年
            LocalizationSettings.SelectedLocale = Locale.CreateLocale("en");
            Assert.AreEqual(ymd.ToString(), "1/23/2025");
        }
    }
}
