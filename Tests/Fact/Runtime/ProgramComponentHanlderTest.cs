using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class ProgramComponentHanlderTest
    {
        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task SeniorMenShortProgram(string basedayString)
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
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    {
                        var errorMessage = "3回転+2回転は構成できません";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "2Lo"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var errorMessage = "同じジャンプを複数構成することはできません";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var errorMessage = "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSSp4"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var errorMessage = "構成要素はすべて設定してください";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task SeniorWomenShortProgram(string basedayString)
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
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    {
                        var errorMessage = "同じジャンプを複数構成することはできません";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        // 同じ要素でもジャンプコンビネーションならエラーにはならない
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                        Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var errorMessage = "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "SSp4"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var errorMessage = "構成要素はすべて設定してください";
                        var programComponentHanlder = ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                        programComponentHanlder.Unset(6, 0);
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        private void SeniorFreeSkating(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {
            {
                var errorMessage = "3連続ジャンプは2番目にオイラーを構成しないと3番目のジャンプはトウループかループかアクセルしか構成できません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // 設定はできるがエラーとなるはず
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2Lo"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
            }
            {
                var errorMessage = "アクセルジャンプを少なくともひとつ構成する必要があります";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // コンビネーション中でもアクセルを含んでいるならOK
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "同じジャンプは3回以上構成できません";
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "1S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 2連続コンビネーションで連続構成はOK
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます";
                // 3回転は3種類以上2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "1T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "1T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4T"));
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4T"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "ジャンプシークェンス(2/3番目にアクセルを含む構成)は最大ひとつまでしか構成できません";
                // 3連続ジャンプシークェンス中に同じジャンプはOK
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                // ジャンプシークェンスは2回以上構成してはいけない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                }
                // 1番目であればジャンプシークエンスにならないので複数構成にアクセルがあっても問題ない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                }
            }
            {
                var errorMessage = "同じスピンを複数構成することはできません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCoSp4"));
                Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "FCoSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(11, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task SeniorMenFreeSkating(string basedayString)
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

            SeniorFreeSkating(() => ProgramComponentHanlderFactory.SeniorMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task SeniorWomenFreeSkating(string basedayString)
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

            SeniorFreeSkating(() => ProgramComponentHanlderFactory.SeniorWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task JuniorMenShortProgram(string basedayString)
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

            {
                var errorMessage = "同じジャンプを複数構成することはできません";
                switch (skateYear)
                {
                    case "2022-23":
                    case "2025-26":
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    case "2023-24":
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    case "2024-25":
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
                {
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(6, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task JuniorWomenShortProgram(string basedayString)
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

            {
                var errorMessage = "同じジャンプを複数構成することはできません";
                switch (skateYear)
                {
                    case "2022-23":
                    case "2025-26":
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    case "2023-24":
                        // "同じジャンプを複数構成することはできません"
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    case "2024-25":
                        // "同じジャンプを複数構成することはできません"
                        {
                            var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                            Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                            Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                            Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                        }
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
                {
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
            }
            {
                var errorMessage = "3回転+2回転は構成できません";
                var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "2Lo"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                programComponentHanlder.Unset(6, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        private void JuniorFreeSkating(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {
            {
                var errorMessage = "3連続ジャンプは2番目にオイラーを構成しないと3番目のジャンプはトウループかループかアクセルしか構成できません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // 設定はできるがエラーとなるはず
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2Lo"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
            }
            {
                var errorMessage = "アクセルジャンプを少なくともひとつ構成する必要があります";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // コンビネーション中でもアクセルを含んでいるならOK
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "同じジャンプは3回以上構成できません";
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "1S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 2連続コンビネーションで連続構成はOK
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます";
                // 3回転は3種類以上2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "1T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "1T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4T"));
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4T"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "ジャンプシークェンス(2/3番目にアクセルを含む構成)は最大ひとつまでしか構成できません";
                // 3連続ジャンプシークェンス中に同じジャンプはOK
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                // ジャンプシークェンスは2回以上構成してはいけない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                }
                // 1番目であればジャンプシークエンスにならないので複数構成にアクセルがあっても問題ない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                }
            }
            {
                var errorMessage = "同じスピンを複数構成することはできません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCoSp4"));
                Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "FCoSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(10, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task JuniorMenFreeSkating(string basedayString)
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

            JuniorFreeSkating(() => ProgramComponentHanlderFactory.JuniorMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task JuniorWomenFreeSkating(string basedayString)
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

            JuniorFreeSkating(() => ProgramComponentHanlderFactory.JuniorWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        private void NoviceAFreeSkating(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {
            {
                var errorMessage = "3連続ジャンプは2番目にオイラーを構成しないと3番目のジャンプはトウループかループかアクセルしか構成できません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // 設定はできるがエラーとなるはず
                Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2Lo"));
                Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2S"));
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(5, 2, "ChSq1"));
            }
            {
                var errorMessage = "アクセルジャンプを少なくともひとつ構成する必要があります";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // コンビネーション中でもアクセルを含んでいるならOK
                Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "同じジャンプは3回以上構成できません";
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "1S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 2連続コンビネーションで連続構成はOK
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます";
                // 3回転は2種類までなら2回繰り返すことができる
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "4F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "4Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "第1ジャンプにループ、ルッツ、フリップの3種類を含めてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // ひとつでもかけてはダメ
                Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "1A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 第1ジャンプ以外ではダメ
                Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 欠けていたのがそろったのでOK
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3T"));
                Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3T"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "ジャンプシークェンス(2/3番目にアクセルを含む構成)は最大ひとつまでしか構成できません";
                // 3連続ジャンプシークェンス中に同じジャンプはOK
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                // ジャンプシークェンスは2回以上構成してはいけない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                }
                // 1番目であればジャンプシークエンスにならないので複数構成にアクセルがあっても問題ない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                }
            }
            {
                var errorMessage = "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "FSSp4"));
                Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "SSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "FCSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(9, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task NoviceAMenFreeSkating(string basedayString)
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

            NoviceAFreeSkating(() => ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task NoviceAWomenFreeSkating(string basedayString)
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

            NoviceAFreeSkating(() => ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        private void NoviceBFreeSkating(Func<ProgramComponentHanlder> CreateProgramComponentHanlder)
        {
            {
                var errorMessage = "3連続ジャンプは2番目にオイラーを構成しないと3番目のジャンプはトウループかループかアクセルしか構成できません";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // 設定はできるがエラーとなるはず
                Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3S"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2Lo"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                // もともと構成できない要素は当然構成できない
                Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "ChSq1"));
            }
            {
                var errorMessage = "アクセルジャンプを少なくともひとつ構成する必要があります";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2S"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // コンビネーション中でもアクセルを含んでいるならOK
                Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "同じジャンプは3回以上構成できません";
                // どのような回転数でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "1S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "1S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "4S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4S"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 2連続コンビネーションで連続構成はOK
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 全体で3回以上の構成はダメ
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 3連続コンビネーション中でもダメ
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3T"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます";
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    // 3回転は2種類までなら2回繰り返すことができる
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 3回転は3種類以上2回繰り返すことはできない
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "3Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3Lz"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "3Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
                // 4回転を2種類2回繰り返すことはできない
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "4F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "4F"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "4Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "4Lo"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                }
            }
            {
                var errorMessage = "第1ジャンプにルッツ、フリップの2種類を含めてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                // ひとつでもかけてはダメ
                Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "1A"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 第1ジャンプ以外ではダメ
                Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "1Eu"));
                Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2F"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 欠けていたのがそろったのでOK
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "2F"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3Lz"));
                Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3F"));
                Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
            {
                var errorMessage = "ジャンプシークェンス(2/3番目にアクセルを含む構成)は最大ひとつまでしか構成できません";
                // 3連続ジャンプシークェンス中に同じジャンプはOK
                {
                    var programComponentHanlder = CreateProgramComponentHanlder();
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                // ジャンプシークェンスは2回以上構成してはいけない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                    }
                }
                // 1番目であればジャンプシークエンスにならないので複数構成にアクセルがあっても問題ない
                {
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                    {
                        var programComponentHanlder = CreateProgramComponentHanlder();
                        Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "2A"));
                        Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2A"));
                        Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    }
                }
            }
            {
                var errorMessage = "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "FSSp4"));
                Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "SSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
                // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "FCSp4"));
                Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
            }
            {
                var errorMessage = "構成要素はすべて設定してください";
                var programComponentHanlder = CreateProgramComponentHanlder();
                programComponentHanlder.Unset(8, 0);
                Assert.AreEqual(programComponentHanlder.ErrorMessage, errorMessage);
            }
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task NoviceBMenFreeSkating(string basedayString)
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

            NoviceBFreeSkating(() => ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }

        [TestCase("2022/7/1")]
        [TestCase("2023/7/1")]
        [TestCase("2024/7/1")]
        [TestCase("2025/7/1")]
        public async Task NoviceBWomenFreeSkating(string basedayString)
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

            NoviceBFreeSkating(() => ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
        }
    }
}
