using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class ScriptableObjectTest
    {
        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task SeasonData(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            await ClassObjects(baseday);
            await CompetitionObjects(baseday);
            await ElementBaseValueObjects(baseday);
            await GoePlusObjects(baseday);
            await GoeMinusObjects(baseday);
            await ProgramComponentRegulationObjects(baseday);
        }

        private async Task ClassObjects(YearMonthDay baseday)
        {
            var allObjs = await ClassObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/Class.csv");
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

        private async Task CompetitionObjects(YearMonthDay baseday)
        {
            var allObjs = await CompetitionObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/Competition.csv");
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

        private async Task ElementBaseValueObjects(YearMonthDay baseday)
        {
            var allObjs = await ElementBaseValueObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/ElementBaseValue.csv");
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

        private async Task GoePlusObjects(YearMonthDay baseday)
        {
            var allObjs = await GoePlusObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/GoePlus.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.description));
            }
        }

        private async Task GoeMinusObjects(YearMonthDay baseday)
        {
            var allObjs = await GoeMinusObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/GoeMinus.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.category));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.description));
                Assert.IsTrue(obj.data.minValue <= 0);
                Assert.IsTrue(obj.data.maxValue <= 0);
                Assert.IsTrue(obj.data.maxValue <= obj.data.minValue);
                // group, mark, targetElementIds はデータなしを許容
            }
        }

        private async Task ProgramComponentRegulationObjects(YearMonthDay baseday)
        {
            var allObjs = await ProgramComponentRegulationObjectQuery.All(baseday);
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/{YearMonthDayUtility.GetSkateYearString(baseday)}/ProgramComponentRegulation.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsNotNull(obj.data.elementPlaceableSetIds);
                Assert.IsTrue(obj.data.elementPlaceableSetIds.Length > 0);
            }
        }

        [Test]
        public async Task ElementObjects()
        {
            var allObjs = await ElementObjectQuery.All();
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/Element.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.goeCategory));
                // upgradeId, downgradeId はデータなしを許容
            }
        }

        [Test]
        public async Task SexObjects()
        {
            var allObjs = await SexObjectQuery.All();
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/Sex.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
            }
        }

        [Test]
        public async Task EventObjects()
        {
            var allObjs = await EventObjectQuery.All();
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/Event.csv");
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

        [Test]
        public async Task ElementPlaceableObjects()
        {
            var allObjs = await ElementPlaceableObjectQuery.All();
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/ElementPlaceable.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsNotNull(obj.data.elementIds);
                Assert.IsTrue(obj.data.elementIds.Length > 0);
                // TODO:配列化した結果が同一なデータの検出テストの追加
            }
        }

        [Test]
        public async Task ElementPlaceableSetObjects()
        {
            var allObjs = await ElementPlaceableSetObjectQuery.All();
            if (allObjs == null) return;
            ObjCountAreEqualCsvDataCount(allObjs, @$"Packages/com.welovegamesinc.figureskate-framework/Fact/MasterData/ElementPlaceableSet.csv");
            foreach (var obj in allObjs)
            {
                Assert.IsNotNull(obj.data);
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.id));
                Assert.IsFalse(string.IsNullOrEmpty(obj.data.name));
                Assert.IsNotNull(obj.data.elementPlaceableIds);
                Assert.IsTrue(obj.data.elementPlaceableIds.Length > 0);
            }
        }

        private void ObjCountAreEqualCsvDataCount<T>(List<T> objs, string masterDataPath)
        {
            var csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(masterDataPath);
            var csvDataArray = CSVSerializer.ParseCSV(csvTextAsset.text);
            var csvDataCount = csvDataArray.Count - 1; // ヘッダ部分を除外
            Assert.AreEqual(objs.Count, csvDataCount);
        }
    }
}
