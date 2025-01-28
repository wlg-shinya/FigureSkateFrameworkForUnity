using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class ScriptableObjectTest
    {
        [TestCase("2024/1/1")]
        public async Task SeasonDataTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            await ClassObjectsTest(baseday);
            await CompetitionObjectsTest(baseday);
            await ElementBaseValueObjectsTest(baseday);
            await GoeObjectsTest(baseday);
        }

        private async Task ClassObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await ClassObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsTrue(obj.data.minAge >= 0);
                Assert.IsTrue(obj.data.maxAge >= 0);
                Assert.IsTrue(obj.data.minAge <= obj.data.maxAge);
            }
        }

        private async Task CompetitionObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await CompetitionObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.regionId));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.countryId));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsNotNull(obj.data.startDay);
                Assert.IsNotNull(obj.data.endDay);
                Assert.IsTrue(obj.data.startDay <= obj.data.endDay);
                Assert.IsNotNull(obj.data.eventIds);
                Assert.IsTrue(obj.data.eventIds.Length > 0);
            }
        }

        private async Task ElementBaseValueObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await ElementBaseValueObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsTrue(obj.data.baseValue >= 0);
                Assert.IsTrue(obj.data.baseValueM5 <= obj.data.baseValueM4);
                Assert.IsTrue(obj.data.baseValueM4 <= obj.data.baseValueM3);
                Assert.IsTrue(obj.data.baseValueM3 <= obj.data.baseValueM2);
                Assert.IsTrue(obj.data.baseValueM2 <= obj.data.baseValueM1);
                Assert.IsTrue(obj.data.baseValueP1 <= obj.data.baseValueP2);
                Assert.IsTrue(obj.data.baseValueP2 <= obj.data.baseValueP3);
                Assert.IsTrue(obj.data.baseValueP3 <= obj.data.baseValueP4);
                Assert.IsTrue(obj.data.baseValueP4 <= obj.data.baseValueP5);
            }
        }

        private async Task GoeObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await GoeObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsNotNull(obj.data.plus);
                Assert.IsTrue(obj.data.plus.Length > 0);
                Assert.IsNotNull(obj.data.minus);
                Assert.IsTrue(obj.data.minus.Length > 0);
            }
        }

        [Test]
        public async Task ElementObjectsTest()
        {
            var allObjs = await ElementObjectQuery.All();
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.goeId));
                // upgradeId と downgradeId は null/空文字許容
            }
        }

        [Test]
        public async Task SexObjectsTest()
        {
            var allObjs = await SexObjectQuery.All();
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
            }
        }

        [Test]
        public async Task EventObjectsTest()
        {
            var allObjs = await EventObjectQuery.All();
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.classId));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.sexId));
                Assert.IsNotNull(obj.data.programIds);
                Assert.IsTrue(obj.data.programIds.Length > 0);
            }
        }
    }
}
