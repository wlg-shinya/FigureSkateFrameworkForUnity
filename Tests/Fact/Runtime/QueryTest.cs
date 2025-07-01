using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Fact;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class QueryTest
    {
        private readonly YearMonthDay BASEDAY_2022_23 = new(2022, 7, 1);
        private readonly YearMonthDay BASEDAY_2023_24 = new(2023, 7, 1);
        private readonly YearMonthDay BASEDAY_2024_25 = new(2024, 7, 1);
        private readonly YearMonthDay BASEDAY_2025_26 = new(2025, 7, 1);

        [Test]
        public async Task Verify()
        {
            var sexObjectAll = await SexObjectQuery.All();
            var eventObjectAll = await EventObjectQuery.All();
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var classObjectAll2022_23 = await ClassObjectQuery.All(BASEDAY_2022_23);
            var programComponentRegulationObjectAll2022_23 = await ProgramComponentRegulationObjectQuery.All(BASEDAY_2022_23);
            var elementBaseValueObjectAll2022_23 = await ElementBaseValueObjectQuery.All(BASEDAY_2022_23);
            var programObjectAll2022_23 = (await ProgramObjectQuery.All(BASEDAY_2022_23)).Select(x => ProgramObjectQuery.SetupConditions(x, programComponentRegulationObjectAll2022_23, elementPlaceableSetObjectAll)).ToList();

            // 単体総当たりチェック
            foreach (var sexId in sexObjectAll.Select(x => x.data.id))
            {
                foreach (var classId in classObjectAll2022_23.Select(x => x.data.id))
                {
                    var eventSeniorMenObject = EventObjectQuery.ByClassIdAndSexId(eventObjectAll, classId, sexId);
                    foreach (var programId in eventSeniorMenObject.data.programIds)
                    {
                        var programObject = ProgramObjectQuery.ById(programObjectAll2022_23, programId);
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, programObject.data.programComponentRegulationId);

                        ProgramComponent[] components = null;

                        // プログラム構成データが不正なら不整合があるはず
                        Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));

                        // 正しく作成された未設定のプログラム構成は特に不整合はないはず
                        components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));

                        // 問題ない構成要素が設定されているなら不整合はないはず
                        // (本来は ProgramComponentHanlder を経由すべき。テスト都合で簡略)
                        components[0].elementIds[0] = "2A";
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));

                        // 不正なデータは不整合があるはず
                        components[0].elementIds[0] = "AAA";
                        Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));

                        // 編集を経て空にデータに不整合はないはず
                        components[0].elementIds[0] = "";
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                }
            }

            var classObjectAll2023_24 = await ClassObjectQuery.All(BASEDAY_2023_24);
            var programComponentRegulationObjectAll2023_24 = await ProgramComponentRegulationObjectQuery.All(BASEDAY_2023_24);
            var elementBaseValueObjectAll2023_24 = await ElementBaseValueObjectQuery.All(BASEDAY_2023_24);
            var programObjectAll2023_24 = (await ProgramObjectQuery.All(BASEDAY_2023_24)).Select(x => ProgramObjectQuery.SetupConditions(x, programComponentRegulationObjectAll2023_24, elementPlaceableSetObjectAll)).ToList();
            var classObjectAll2024_25 = await ClassObjectQuery.All(BASEDAY_2024_25);
            var programComponentRegulationObjectAll2024_25 = await ProgramComponentRegulationObjectQuery.All(BASEDAY_2024_25);
            var elementBaseValueObjectAll2024_25 = await ElementBaseValueObjectQuery.All(BASEDAY_2024_25);
            var programObjectAll2024_25 = (await ProgramObjectQuery.All(BASEDAY_2024_25)).Select(x => ProgramObjectQuery.SetupConditions(x, programComponentRegulationObjectAll2024_25, elementPlaceableSetObjectAll)).ToList();
            var classObjectAll2025_26 = await ClassObjectQuery.All(BASEDAY_2025_26);
            var programComponentRegulationObjectAll2025_26 = await ProgramComponentRegulationObjectQuery.All(BASEDAY_2025_26);
            var elementBaseValueObjectAll2025_26 = await ElementBaseValueObjectQuery.All(BASEDAY_2025_26);
            var programObjectAll2025_26 = (await ProgramObjectQuery.All(BASEDAY_2025_26)).Select(x => ProgramObjectQuery.SetupConditions(x, programComponentRegulationObjectAll2022_23, elementPlaceableSetObjectAll)).ToList();
            // シーズンの変更チェック
            {
                // 2025-26 -> 2024-25
                {
                    // ジュニアのショートプログラムでは課題が違うので不整合が出るはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "JuniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                    // 上記以外では不整合は出ないはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "SeniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2025_26, ProgramObjectQuery.ById(programObjectAll2025_26, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                    }
                }
                // 2024-25 -> 2023-24
                {
                    // 2024-25にしか存在しない構成要素を設定している場合は不整合が出るはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        components[0].elementIds[0] = "5S";
                    }
                    // ジュニアのショートプログラムでは課題が違うので不整合が出るはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "JuniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                    // 上記以外では不整合は出ないはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "SeniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2023_24));
                    }
                }
                // 2023-24 -> 2022-23
                {
                    // ジュニアのショートプログラムでは課題が違うので不整合が出るはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "JuniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "JuniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                    // 上記以外では不整合は出ないはず
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "SeniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "JuniorMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "SeniorMenShortProgram").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "SeniorMenShortProgram").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                    {
                        var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2023_24, ProgramObjectQuery.ById(programObjectAll2023_24, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                        var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2022_23, ProgramObjectQuery.ById(programObjectAll2022_23, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                        Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2022_23));
                    }
                }
            }
            // クラスの変更チェック
            {
                // 今のところクラスの変更は常に不整合が発生する

                // シニアフリースケーティング -> ジュニアフリースケーティング
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorWomenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                // ジュニアフリースケーティング -> ノービスAフリースケーティング
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorWomenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                // ノービスAフリースケーティング -> ノービスBフリースケーティング
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAWomenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
            }
            // 性別の変更チェック
            {
                // シニア・ジュニアのフリースケーティングは男女に違いがないので不整合はないはず
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsTrue(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                // シニア・ジュニアのショートプログラムは男女に違いがあるので不整合があるはず
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorMenShortProgram").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "SeniorWomenShortProgram").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorMenShortProgram").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "JuniorWomenShortProgram").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                // ノービスA/Bは男女に違いがあるので不整合があるはず
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceAWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
                {
                    var regulationObject = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBMenFreeSkating").data.programComponentRegulationId);
                    var components = ProgramComponentQuery.Create(regulationObject.data, elementPlaceableSetObjectAll);
                    var regulationObject2 = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll2024_25, ProgramObjectQuery.ById(programObjectAll2024_25, "NoviceBWomenFreeSkating").data.programComponentRegulationId);
                    Assert.IsFalse(ProgramComponentQuery.Verify(components, regulationObject2.data, elementPlaceableSetObjectAll, elementPlaceableObjectAll, elementBaseValueObjectAll2024_25));
                }
            }
        }
    }
}
