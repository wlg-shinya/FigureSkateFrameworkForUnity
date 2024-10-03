using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private string SkateYear() => YearMonthDayUtility.GetSkateYearString(baseday);

        [Test]
        public void CheckTesGoePlusValue()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = ElementObjectLoader.All(baseday);
            var judge = new Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckTesGoePlusValue", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);
            var elementId = programComponentHanlder.ProgramComponents[0].elementIds[0];
            var elementObject = elementObjectAll.Find(x => Equals(x.data.id, elementId)) ?? throw new Exception($"Not found '{elementId}'");

            // 全項目成功ならGOE+5
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, ElementObject elementObject) =>
                {
                    return true;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, successGoePlus });
                Assert.AreEqual(returnValue, 5);
            }
            // 一つも成功していないならGOE+0
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, ElementObject elementObject) =>
                {
                    return false;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, successGoePlus });
                Assert.AreEqual(returnValue, 0);
            }
            // 重要項目だけ全部成功すると3項目あるのでGOE+3
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, ElementObject elementObject) =>
                {
                    return goePlus.important;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
            // 重要でない項目だけ全部成功すると3項目あるのでGOE+3
            {
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, ElementObject elementObject) =>
                {
                    return !goePlus.important;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
            // 重要項目が一つでも失敗するとそれ以外の項目が満点でもGOE+3
            {
                var importantTrueCount = 2;
                Judge.SuccessGoePlus successGoePlus = (GoePlus goePlus, ElementObject elementObject) =>
                {
                    importantTrueCount--;
                    return !goePlus.important || importantTrueCount >= 0;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, successGoePlus });
                Assert.AreEqual(returnValue, 3);
            }
        }

        [Test]
        public void CheckTesGoeMinusValue()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = ElementObjectLoader.All(baseday);
            var judge = new Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
            Assert.IsNotNull(judge);
            var methodInfo = judge.GetType().GetMethod("CheckTesGoeMinusValue", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(methodInfo);
            ElementObject elementObject = null;
            {
                elementObject = ElementObjectQuery.ById(elementObjectAll, "2A");
                Assert.IsNotNull(elementObject);
            }

            // 減点項目に引っかからなかったらGOE-0 & 減点項目も0
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, ElementObject elementObject) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.minValue;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, goeMinus, successGoeMinus, checkGoeMinusValue });
                Assert.AreEqual(returnValue, 0);
                Assert.AreEqual(goeMinus.Count, 0);
            }
            // すべての減点項目に引っかかった場合、全減点合計か最低GOEのどちらか大きいほうがGOEになる
            // 減点項目数は減点の仕組み上の最大値と等しくなる
            {
                var goeMinusValueTotal = 0;
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, ElementObject elementObject) =>
                {
                    return false;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    goeMinusValueTotal += goeMinus.maxValue;
                    return goeMinus.maxValue;
                };
                var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, goeMinus, successGoeMinus, checkGoeMinusValue });
                Assert.AreEqual(returnValue, Math.Max(goeMinusValueTotal, Constant.GOE_MIN_VALUE));

                // 減点の仕組み上の項目最大数 ( ref. Judge.CheckTesGoeMinusValue ) との比較
                var geoMinusMaxCount = elementObject.goeObject.data.minus
                    .Where((x) => x.targetElementIds.Length <= 0 || x.targetElementIds.Any(x => Equals(x, elementObject.data.id)))
                    .Where((x) => !(!elementObject.downgrade && x.isDowngrade))
                    .GroupBy((x) => x.group)
                    .SelectMany((x) => Equals(x.Key, "") ? x.ToArray() : new GoeMinus[] { x.Aggregate((a, c) => a.TotalValue() < c.TotalValue() ? a : c) })
                    .Count();
                Assert.AreEqual(goeMinus.Count, geoMinusMaxCount);
            }
            // 上限範囲外テスト
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, ElementObject elementObject) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.maxValue + 1;
                };
                try
                {
                    var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, goeMinus, successGoeMinus, checkGoeMinusValue });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
            // 下限範囲外テスト
            {
                var goeMinus = new List<GoeMinus>();
                Judge.SuccessGoeMinus successGoeMinus = (GoeMinus goeMinus, ElementObject elementObject) =>
                {
                    return true;
                };
                Judge.CheckGoeMinusValue checkGoeMinusValue = (GoeMinus goeMinus) =>
                {
                    return goeMinus.minValue - 1;
                };
                try
                {
                    var returnValue = (int)methodInfo.Invoke(judge, new object[] { elementObject, goeMinus, successGoeMinus, checkGoeMinusValue });
                    Assert.IsTrue(false); // ここには到達しないはず
                }
                catch { }
            }
        }

        [Test]
        public void CheckTesJump()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = ElementObjectLoader.All(baseday);
            var judge = new Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
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
        public void RecordTes()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = ElementObjectLoader.All(baseday);
            var judge = new Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
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
                            var elementObject = elementObjectAll.Find(x => Equals(x.data.id, elementId)) ?? throw new Exception($"Not found '{elementId}'");
                            goeMinus[refereeIndex][elementIndex].Add(elementObject.goeObject.data.minus.ToList().Find(x => x.isDowngrade));
                        }
                    }
                    methodInfo.Invoke(judge, new object[] { tes, component, goeMinus });
                    // 情報記号が想定通りか
                    Assert.AreEqual(tes.executedElement, "2A<<+3Lo");
                    // 一つ目のジャンプがダウングレードしたので基礎点がダウングレード先の値を参照した結果になっているはず
                    var elementObject0 = elementObjectAll.Find(x => Equals(x.data.id, component.elementIds[0])) ?? throw new Exception($"Not found '{component.elementIds[0]}'");
                    var elementObject1 = elementObjectAll.Find(x => Equals(x.data.id, component.elementIds[1])) ?? throw new Exception($"Not found '{component.elementIds[1]}'");
                    Assert.AreEqual(tes.baseValue, elementObject0.downgrade.data.baseValue + elementObject1.data.baseValue);
                }
            }
        }

        [Test]
        public void CheckPcs()
        {
            ProgramComponentHanlder programComponentHanlder = null;
            {
                programComponentHanlder = CreateProgramComponentHanlderAsync();
                Assert.IsNotNull(programComponentHanlder);
            }
            var elementObjectAll = ElementObjectLoader.All(baseday);
            var judge = new Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
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

        private ProgramComponentHanlder CreateProgramComponentHanlderAsync()
        {
            var competitionObjectAll = CompetitionObjectLoader.All(baseday);
            CompetitionObject competitionObject = CompetitionObjectQuery.ById(competitionObjectAll, "KinoshitaGroupCupJapanOpen2023");
            ProgramComponentHanlder programComponentHanlderHanlder;
            {
                var programObject = competitionObject.eventObjects
                    .Select(eventObject => eventObject.programObjects.Find(programObject => Equals(programObject.name, "SeniorMenShortProgram")))
                    .First();
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
