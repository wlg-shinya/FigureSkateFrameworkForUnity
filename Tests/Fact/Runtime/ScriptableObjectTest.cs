using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    // TODO:データ数とファイル数が一致するかどうかのテストの追加
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
            await GoePlusObjectsTest(baseday);
            await GoeMinusObjectsTest(baseday);
            await ElementPlaceableObjectsTest(baseday);
            await ElementPlaceableSetObjectsTest(baseday);
            await ProgramComponentRegulationObjectsTest(baseday);
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
                Assert.IsNotNull(obj.data.plusIds);
                Assert.IsTrue(obj.data.plusIds.Length > 0);
                Assert.IsNotNull(obj.data.minusIds);
                Assert.IsTrue(obj.data.minusIds.Length > 0);
            }
        }

        private async Task GoePlusObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await GoePlusObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.description));
            }
        }

        private async Task GoeMinusObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await GoeMinusObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.description));
                Assert.IsTrue(obj.data.minValue <= 0);
                Assert.IsTrue(obj.data.maxValue <= 0);
                Assert.IsTrue(obj.data.maxValue <= obj.data.minValue);
                // group, mark, targetElementIds はデータなしを許容
            }
        }

        private async Task ElementPlaceableObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await ElementPlaceableObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsNotNull(obj.data.elementIds);
                Assert.IsTrue(obj.data.elementIds.Length > 0);
                // TODO:配列化した結果が同一なデータの検出テストの追加
            }
        }

        private async Task ElementPlaceableSetObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await ElementPlaceableSetObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsNotNull(obj.data.elementPlaceableIds);
                Assert.IsTrue(obj.data.elementPlaceableIds.Length > 0);
            }
        }

        private async Task ProgramComponentRegulationObjectsTest(YearMonthDay baseday)
        {
            var allObjs = await ProgramComponentRegulationObjectQuery.All(baseday);
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsNotNull(obj.data.elementPlaceableSetIds);
                Assert.IsTrue(obj.data.elementPlaceableSetIds.Length > 0);
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
                // upgradeId, downgradeId はデータなしを許容
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
