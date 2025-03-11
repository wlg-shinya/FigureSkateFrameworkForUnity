using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class ProgramComponentHanlderTest
    {
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task SeniorMenShortProgramTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);

            // プログラムの規則をセットアップ
            var programObject = programObjectAll.Find(x => x.data.id.Equals("SeniorMenShortProgram"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);

            // プログラム構成の構築
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);

            // 各オブジェクトの配列化
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            switch (skateYear)
            {
                case "2023-24":
                case "2024-25":
                    // "3回転+2回転は構成できません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(2, 0, "3A");
                        Assert.IsFalse(programComponentHanlder.TrySet(2, 1, "2Lo"));
                    }

                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(0, 0, "3A");
                        programComponentHanlder.TrySet(2, 0, "3A");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(3, 0, "FSSp4");
                        programComponentHanlder.TrySet(4, 0, "CSSp4");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                        programComponentHanlder.TrySet(3, 0, "FCSp4");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task SeniorWomenShortProgramTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("SeniorWomenShortProgram"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            switch (skateYear)
            {
                case "2023-24":
                case "2024-25":
                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(0, 0, "3A");
                        programComponentHanlder.TrySet(2, 0, "3A");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(3, 0, "FSSp4");
                        programComponentHanlder.TrySet(4, 0, "SSp4");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                        programComponentHanlder.TrySet(3, 0, "FCSp4");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        private void SeniorFreeSkatingTest(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {
            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                // var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(6, 0, "3S");
                programComponentHanlder.TrySet(6, 1, "2Lo");
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(6, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "2S"));
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(6, 0, "3S");
                programComponentHanlder.TrySet(6, 1, "1Eu");
                programComponentHanlder.TrySet(6, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(0, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    programComponentHanlder.TrySet(5, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(5, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3F");
                    programComponentHanlder.TrySet(4, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    programComponentHanlder.TrySet(5, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(0, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(6, 0, "3T");
                    programComponentHanlder.TrySet(6, 1, "3T");
                    programComponentHanlder.TrySet(6, 2, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                // 3回転は3種類以上2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(6, 0, "1S");
                    programComponentHanlder.TrySet(0, 0, "4Lz");
                    programComponentHanlder.TrySet(4, 0, "4Lz");
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(2, 0, "1T");
                    programComponentHanlder.TrySet(5, 0, "1T");
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(6, 0, "1S");
                    programComponentHanlder.TrySet(2, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(6, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(0, 0, "4T");
                programComponentHanlder.TrySet(2, 0, "4T");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じスピンを複数構成することはできません"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(7, 0, "FCoSp4");
                programComponentHanlder.TrySet(9, 0, "FCoSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(11, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task SeniorMenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("SeniorMenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            SeniorFreeSkatingTest(() => ProgramComponentHanlderFactory.SeniorMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task SeniorWomenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("SeniorWomenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            SeniorFreeSkatingTest(() => ProgramComponentHanlderFactory.SeniorWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task JuniorMenShortProgramTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("JuniorMenShortProgram"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            switch (skateYear)
            {
                case "2023-24":
                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(1, 0, "3Lz");
                        programComponentHanlder.TrySet(2, 0, "3Lz");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                case "2024-25":
                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(1, 0, "3F");
                        programComponentHanlder.TrySet(2, 0, "3F");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task JuniorWomenShortProgramTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("JuniorWomenShortProgram"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            switch (skateYear)
            {
                case "2023-24":
                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(1, 0, "3Lz");
                        programComponentHanlder.TrySet(2, 0, "3Lz");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                case "2024-25":
                    // "同じジャンプを複数構成することはできません"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.TrySet(1, 0, "3F");
                        programComponentHanlder.TrySet(2, 0, "3F");
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        programComponentHanlder.TrySet(2, 0, "3Lo");
                        programComponentHanlder.TrySet(2, 1, "3Lo");
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }

                    // "構成要素はすべて設定してください"
                    {
                        var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        private void JuniorFreeSkatingTest(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {

            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(6, 0, "3S");
                programComponentHanlder.TrySet(6, 1, "2Lo");
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(6, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(1, 0, "2S");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(6, 0, "3S");
                programComponentHanlder.TrySet(6, 1, "1Eu");
                programComponentHanlder.TrySet(6, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(0, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    programComponentHanlder.TrySet(5, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(5, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3F");
                    programComponentHanlder.TrySet(4, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    programComponentHanlder.TrySet(5, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(0, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(6, 0, "3T");
                    programComponentHanlder.TrySet(6, 1, "3T");
                    programComponentHanlder.TrySet(6, 2, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                // 3回転は3種類以上2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(6, 0, "1S");
                    programComponentHanlder.TrySet(0, 0, "4Lz");
                    programComponentHanlder.TrySet(4, 0, "4Lz");
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    programComponentHanlder.TrySet(2, 0, "1T");
                    programComponentHanlder.TrySet(5, 0, "1T");
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(6, 0, "1S");
                    programComponentHanlder.TrySet(2, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(6, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(0, 0, "4T");
                programComponentHanlder.TrySet(2, 0, "4T");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じスピンを複数構成することはできません"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.TrySet(7, 0, "FCoSp4");
                programComponentHanlder.TrySet(9, 0, "FCoSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(10, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task JuniorMenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("JuniorMenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            JuniorFreeSkatingTest(() => ProgramComponentHanlderFactory.JuniorMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task JuniorWomenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("JuniorWomenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            JuniorFreeSkatingTest(() => ProgramComponentHanlderFactory.JuniorWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task NoviceAMenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("NoviceAMenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 2, "2T");
                programComponentHanlder.TrySet(5, 0, "3F");
                programComponentHanlder.TrySet(5, 1, "2Lo");
                Assert.IsFalse(programComponentHanlder.TrySet(5, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(5, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(5, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 2, "2S");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(5, 0, "3F");
                programComponentHanlder.TrySet(5, 1, "1Eu");
                programComponentHanlder.TrySet(5, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    programComponentHanlder.TrySet(5, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(5, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(4, 0, "3S");
                    programComponentHanlder.TrySet(5, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    programComponentHanlder.TrySet(5, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "1T");
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 2, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                // 3回転は2種類までなら2回繰り返すことができる
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4Lo");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "第1ジャンプにループ、ルッツ、フリップの3種類を含めてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);

                // ひとつでもかけてはダメ
                programComponentHanlder.TrySet(0, 0, "3Lz");
                programComponentHanlder.TrySet(1, 0, "3F");
                programComponentHanlder.TrySet(2, 0, "1A");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 第1ジャンプ以外ではダメ
                programComponentHanlder.TrySet(4, 1, "3Lo");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 欠けていたのがそろったのでOK
                programComponentHanlder.TrySet(2, 0, "3Lo");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(1, 0, "3T");
                programComponentHanlder.TrySet(3, 0, "3T");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(6, 0, "FSSp4");
                programComponentHanlder.TrySet(7, 0, "SSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                programComponentHanlder.TrySet(6, 0, "FCSp4");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(9, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task NoviceAWomenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("NoviceAWomenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 2, "2T");
                programComponentHanlder.TrySet(5, 0, "3F");
                programComponentHanlder.TrySet(5, 1, "2Lo");
                Assert.IsFalse(programComponentHanlder.TrySet(5, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(5, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(5, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 2, "2S");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(5, 0, "3F");
                programComponentHanlder.TrySet(5, 1, "1Eu");
                programComponentHanlder.TrySet(5, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    programComponentHanlder.TrySet(5, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(5, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(4, 0, "3S");
                    programComponentHanlder.TrySet(5, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    programComponentHanlder.TrySet(5, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "1T");
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 2, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 3回転は2種類までなら2回繰り返すことができる
                    programComponentHanlder.TrySet(1, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4Lo");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "第1ジャンプにループ、ルッツ、フリップの3種類を含めてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                // ひとつでもかけてはダメ
                programComponentHanlder.TrySet(0, 0, "3Lz");
                programComponentHanlder.TrySet(1, 0, "3F");
                programComponentHanlder.TrySet(2, 0, "1A");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 第1ジャンプ以外ではダメ
                programComponentHanlder.TrySet(4, 1, "3Lo");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 欠けていたのがそろったのでOK
                programComponentHanlder.TrySet(2, 0, "3Lo");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(1, 0, "3T");
                programComponentHanlder.TrySet(3, 0, "3T");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(6, 0, "FSSp4");
                programComponentHanlder.TrySet(7, 0, "SSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                programComponentHanlder.TrySet(6, 0, "FCSp4");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(9, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task NoviceBMenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("NoviceBMenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "2Lo");
                programComponentHanlder.TrySet(4, 2, "2T");
                Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(4, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(2, 0, "2S");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "1Eu");
                programComponentHanlder.TrySet(4, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(2, 0, "1S");
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "2S");
                    programComponentHanlder.TrySet(3, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "3S");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(4, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4S");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 2連続コンビネーションで連続構成はOK
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(3, 0, "3T");
                    programComponentHanlder.TrySet(3, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 2, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 3回転は2種類までなら2回繰り返すことができる
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    programComponentHanlder.TrySet(1, 0, "3F");
                    programComponentHanlder.TrySet(4, 0, "3F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    programComponentHanlder.TrySet(4, 2, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "第1ジャンプにルッツ、フリップの2種類を含めてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                // ひとつでもかけてはダメ
                programComponentHanlder.TrySet(0, 0, "3Lz");
                programComponentHanlder.TrySet(1, 0, "1A");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 第1ジャンプ以外ではダメ
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "1Eu");
                programComponentHanlder.TrySet(4, 2, "2F");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 欠けていたのがそろったのでOK
                programComponentHanlder.TrySet(1, 0, "2F");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(3, 0, "3Lz");
                programComponentHanlder.TrySet(0, 0, "3F");
                programComponentHanlder.TrySet(1, 0, "3F");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 0, "FSSp4");
                programComponentHanlder.TrySet(6, 0, "SSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                programComponentHanlder.TrySet(5, 0, "FCSp4");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(8, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }

        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        public async Task NoviceBWomenFreeSkatingTest(string basedayString)
        {
            var baseday = new YearMonthDay(basedayString);
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var programObject = programObjectAll.Find(x => x.data.id.Equals("NoviceBWomenFreeSkating"));
            ProgramObjectQuery.SetupConditions(programObject, programComponentRegulationObjectAll, elementPlaceableSetObjectAll);
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
            var programComponents = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll);
            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();

            // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "2Lo");
                programComponentHanlder.TrySet(4, 2, "2T");
                Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "2A"));
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                programComponentHanlder.TrySet(4, 1, "1Eu");
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "ChSq1"));
            }

            // "アクセルジャンプを少なくともひとつ構成する必要があります"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(2, 0, "2S");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // コンビネーション中でもアクセルを含んでいるならOK
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "1Eu");
                programComponentHanlder.TrySet(4, 2, "2A");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "同じジャンプは3回以上構成できません"
            {
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(2, 0, "1S");
                    programComponentHanlder.TrySet(3, 0, "1S");
                    programComponentHanlder.TrySet(4, 0, "1S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "2S");
                    programComponentHanlder.TrySet(3, 0, "2S");
                    programComponentHanlder.TrySet(4, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "3S");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(4, 0, "3S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4S");
                    programComponentHanlder.TrySet(3, 0, "4S");
                    programComponentHanlder.TrySet(4, 0, "4S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "3T");
                    programComponentHanlder.TrySet(3, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 2, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
            {
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    // 3回転は2種類までなら2回繰り返すことができる
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    programComponentHanlder.TrySet(1, 0, "3F");
                    programComponentHanlder.TrySet(4, 0, "3F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    programComponentHanlder.TrySet(4, 2, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                }
            }

            // "第1ジャンプにルッツ、フリップの2種類を含めてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                // ひとつでもかけてはダメ
                programComponentHanlder.TrySet(0, 0, "3Lz");
                programComponentHanlder.TrySet(1, 0, "1A");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 第1ジャンプ以外ではダメ
                programComponentHanlder.TrySet(4, 0, "2S");
                programComponentHanlder.TrySet(4, 1, "1Eu");
                programComponentHanlder.TrySet(4, 2, "2F");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 欠けていたのがそろったのでOK
                programComponentHanlder.TrySet(1, 0, "2F");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(3, 0, "3Lz");
                programComponentHanlder.TrySet(0, 0, "3F");
                programComponentHanlder.TrySet(1, 0, "3F");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.TrySet(5, 0, "FSSp4");
                programComponentHanlder.TrySet(6, 0, "SSp4");
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                programComponentHanlder.TrySet(5, 0, "FCSp4");
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }

            // "構成要素はすべて設定してください"
            {
                var programComponentHanlder = ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(8, 0);
                Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
            }
        }
    }
}
