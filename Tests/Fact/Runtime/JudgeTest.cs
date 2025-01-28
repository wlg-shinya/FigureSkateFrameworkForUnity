using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class JudgeTest
    {
        private readonly YearMonthDay baseday = new(2024, 1, 1);

        [Test]
        public async Task CheckTesGoePlusValue()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = await CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = await ElementObjectQuery.All();
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var judge = new Judge(
                programComponentHanlder.Program,
                programComponentHanlder.ProgramComponents,
                elementObjectAll.Select(x => x.data).ToList(),
                elementBaseValueObjectAll.Select(x => x.data).ToList(),
                goeObjectAll.Select(x => x.data).ToList()
                );
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckTesGoePlusValue", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);
            var elementId = programComponentHanlder.ProgramComponents[0].elementIds[0];
            var elementObject = ElementObjectQuery.ById(elementObjectAll, elementId);

            // 全項目成功ならGOE+5
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, Element element) =>
                {
                    return true;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, successGoePlus });
                Assert.AreEqual(returnValue, 5);
            }
            // 一つも成功していないならGOE+0
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, Element element) =>
                {
                    return false;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, successGoePlus });
                Assert.AreEqual(returnValue, 0);
            }
            // 重要項目だけ全部成功すると3項目あるのでGOE+3
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, Element element) =>
                {
                    return goePlus.important;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
            // 重要でない項目だけ全部成功すると3項目あるのでGOE+3
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, Element element) =>
                {
                    return !goePlus.important;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
            // 重要項目が一つでも失敗するとそれ以外の項目が満点でもGOE+3
            {
                var importantTrueCount = 2;
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, Element element) =>
                {
                    importantTrueCount--;
                    return !goePlus.important || importantTrueCount >= 0;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
        }

        [Test]
        public async Task CheckTesGoeMinusValue()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = await CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = await ElementObjectQuery.All();
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var judge = new Judge(
                programComponentHanlder.Program,
                programComponentHanlder.ProgramComponents,
                elementObjectAll.Select(x => x.data).ToList(),
                elementBaseValueObjectAll.Select(x => x.data).ToList(),
                goeObjectAll.Select(x => x.data).ToList()
                );
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckTesGoeMinusValue", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);
            ElementObject elementObject = ElementObjectQuery.ById(elementObjectAll, "2A");

            // 減点項目に引っかからなかったらGOE-0 & 減点項目も0
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, Element element) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.minValue;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, goeMinus, successGoeMinus, checkGoeMinusValue });
                Assert.AreEqual(returnValue, 0);
                Assert.AreEqual(goeMinus.Count, 0);
            }
            // すべての減点項目に引っかかった場合、全減点合計か最低GOEのどちらか大きいほうがGOEになる
            // 減点項目数は減点の仕組み上の最大値と等しくなる
            {
                var goeMinusValueTotal = 0;
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, Element element) =>
                {
                    return false;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    goeMinusValueTotal += goeMinus.maxValue;
                    return goeMinus.maxValue;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, goeMinus, successGoeMinus, checkGoeMinusValue });
                Assert.AreEqual(returnValue, Math.Max(goeMinusValueTotal, Constant.GOE_MIN_VALUE));

                // 減点の仕組み上の項目最大数 ( ref. Judge.CheckTesGoeMinusValue ) との比較
                var goeObject = GoeObjectQuery.ById(goeObjectAll, elementObject.data.goeId);
                var geoMinusMaxCount = goeObject.data.minus
                    .Where((x) => x.targetElementIds.Length <= 0 || x.targetElementIds.Any(x => Equals(x, elementObject.data.id)))
                    .GroupBy((x) => x.group)
                    .SelectMany((x) => Equals(x.Key, "") ? x.ToArray() : new GoeMinus[] { x.Aggregate((a, c) => a.TotalValue() < c.TotalValue() ? a : c) })
                    .Count();
                Assert.AreEqual(goeMinus.Count, geoMinusMaxCount);
            }
            // 上限範囲外テスト
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, Element element) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.maxValue + 1;
                };
                try
                {
                    var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, goeMinus, successGoeMinus, checkGoeMinusValue });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
            // 下限範囲外テスト
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, Element element) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.minValue - 1;
                };
                try
                {
                    var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject.data, goeMinus, successGoeMinus, checkGoeMinusValue });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
        }

        [Test]
        public async Task CheckTesJump()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = await CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = await ElementObjectQuery.All();
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var judge = new Judge(
                programComponentHanlder.Program,
                programComponentHanlder.ProgramComponents,
                elementObjectAll.Select(x => x.data).ToList(),
                elementBaseValueObjectAll.Select(x => x.data).ToList(),
                goeObjectAll.Select(x => x.data).ToList()
                );
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckTesJump", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);

            // ジャンプ失敗時は全審判のGOEは最低点
            {
                // 失敗前はGOE初期値なはず
                Assert.IsTrue(judge.Detail.tes[0].refereeGoe.All(x => x == 0));
                var tes = judge.Detail.tes[0];
                Judge.SuccessJump successJump = () =>
                {
                    return false;
                };
                methodInfo.Invoke(judge, new object[] { tes, successJump });
                Assert.IsTrue(tes.refereeGoe.All(x => x == Constant.GOE_MIN_VALUE));
            }
        }

        [Test]
        public async Task RecordTes()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = await CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = await ElementObjectQuery.All();
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var judge = new Judge(
                programComponentHanlder.Program,
                programComponentHanlder.ProgramComponents,
                elementObjectAll.Select(x => x.data).ToList(),
                elementBaseValueObjectAll.Select(x => x.data).ToList(),
                goeObjectAll.Select(x => x.data).ToList()
                );
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("RecordTes", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);

            // コンビネーションジャンプテスト
            {
                // 後半ジャンプボーナスがかからないように入れ替え
                programComponentHanlder.Swap(2, 1);

                var componentIndex = 1;
                var tes = judge.Detail.tes[componentIndex];
                var component = programComponentHanlder.ProgramComponents[componentIndex];
                var goeMinus = new List<GoeMinus>[Constant.REFEREE_COUNT][];
                for (var refereeIndex = 0; refereeIndex < Constant.REFEREE_COUNT; refereeIndex++)
                {
                    goeMinus[refereeIndex] = new List<GoeMinus>[component.elementIds.Length];
                    for (var elementIndex = 0; elementIndex < component.elementIds.Length; elementIndex++)
                    {
                        goeMinus[refereeIndex][elementIndex] = new List<GoeMinus>();
                        // コンビネーションジャンプの一つ目の要素をダウングレードさせる
                        if (elementIndex == 0)
                        {
                            var elementId = component.elementIds[elementIndex];
                            var elementObject = ElementObjectQuery.ById(elementObjectAll, elementId);
                            var goeObject = GoeObjectQuery.ById(goeObjectAll, elementObject.data.goeId);
                            goeMinus[refereeIndex][elementIndex].Add(goeObject.data.minus.ToList().Find(x => x.mark.Equals("<<")));
                        }
                    }
                    methodInfo.Invoke(judge, new object[] { tes, component, goeMinus });
                    // 情報記号が想定通りか
                    Assert.AreEqual(tes.executedElement, "2A<<+3Lo");
                    // 合計基礎点はダウングレードした値と一致するはず
                    var element0BaseValueObject = ElementBaseValueObjectQuery.ById(elementBaseValueObjectAll, component.elementIds[0] + "<<");
                    var element1BaseValueObject = ElementBaseValueObjectQuery.ById(elementBaseValueObjectAll, component.elementIds[1]);
                    Assert.AreEqual(tes.baseValue, element0BaseValueObject.data.baseValue + element1BaseValueObject.data.baseValue);
                }
            }
        }

        [Test]
        public async Task CheckPcs()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = await CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = await ElementObjectQuery.All();
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var judge = new Judge(
                programComponentHanlder.Program,
                programComponentHanlder.ProgramComponents,
                elementObjectAll.Select(x => x.data).ToList(),
                elementBaseValueObjectAll.Select(x => x.data).ToList(),
                goeObjectAll.Select(x => x.data).ToList()
                );
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckPcs", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);

            {
                Judge.CheckComposition checkComposition = () =>
                {
                    return 0.0f;
                };
                Judge.CheckPresentation checkPresentation = () =>
                {
                    return 1.0f;
                };
                Judge.CheckSkatingSkills checkSkatingSkills = () =>
                {
                    return 0.54f;
                };
                methodInfo.Invoke(judge, new object[] { checkComposition, checkPresentation, checkSkatingSkills });
                // 最低点
                Assert.AreApproximatelyEqual(judge.Detail.pcs[0].RefereeScoreAverage(), 0.0f);
                // 最高点
                Assert.AreApproximatelyEqual(judge.Detail.pcs[1].RefereeScoreAverage(), 10.0f);
                // 0.25刻み四捨五入によるルールによる 5.4 -> 5.5 への丸め
                Assert.AreApproximatelyEqual(judge.Detail.pcs[2].RefereeScoreAverage(), 5.5f);
            }
            // 上限範囲外テスト
            {
                Judge.CheckComposition checkComposition = () =>
                {
                    return 1.1f;
                };
                Judge.CheckPresentation checkPresentation = () => 0.0f;
                Judge.CheckSkatingSkills checkSkatingSkills = () => 0.0f;
                try
                {
                    methodInfo.Invoke(judge, new object[] { checkComposition, checkPresentation, checkSkatingSkills });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
            // 下限範囲外テスト
            {
                Judge.CheckComposition checkComposition = () =>
                {
                    return -0.1f;
                };
                Judge.CheckPresentation checkPresentation = () => 0.0f;
                Judge.CheckSkatingSkills checkSkatingSkills = () => 0.0f;
                try
                {
                    methodInfo.Invoke(judge, new object[] { checkComposition, checkPresentation, checkSkatingSkills });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
        }

        private async Task<ProgramComponentHanlder> CreateProgramComponentHanlderAsync()
        {
            var competitionObjectAll = await CompetitionObjectQuery.All(baseday);
            var eventObjectAll = await EventObjectQuery.All();
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            CompetitionObject competitionObject = CompetitionObjectQuery.ById(competitionObjectAll, "KinoshitaGroupCupJapanOpen2023");
            ProgramComponentHanlder programComponentHanlderHanlder;
            {
                var programObject = ProgramObjectQuery.ById(
                    programObjectAll,
                    competitionObject.data.eventIds
                        .Select(x => EventObjectQuery.ById(eventObjectAll, x))
                        .Select(eventObject => eventObject.data.programIds.Where(programId => Equals(programId, "SeniorMenShortProgram")).First())
                        .First()
                    );
                programObject = ProgramObjectQuery.SetupConditions(programObject);
                var programComponents = ProgramComponentQuery.Create(programObject.data);
                programComponentHanlderHanlder = new ProgramComponentHanlder(programObject.data, programComponents);
                Assert.IsNotNull(programComponentHanlderHanlder);
            }
            programComponentHanlderHanlder.TrySet(0, 0, "3A");
            programComponentHanlderHanlder.TrySet(1, 0, "4Lz");
            programComponentHanlderHanlder.TrySet(2, 0, "2A");
            programComponentHanlderHanlder.TrySet(2, 1, "3Lo");
            return programComponentHanlderHanlder;
        }
    }
}
