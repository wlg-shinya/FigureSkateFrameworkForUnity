using System.Threading.Tasks;
using Wlg.FigureSkate.Core;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public static class ProgramComponentHanlderFactory
    {
        public static async Task<ProgramComponentHandler> SeniorMenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "4Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> SeniorWomenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> SeniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 1, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 1, "2Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 2, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(8, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(10, 0, "StSqB"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(11, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> SeniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return await SeniorMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static async Task<ProgramComponentHandler> JuniorMenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2023-24":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2024-25":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> JuniorWomenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2023-24":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2024-25":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "CSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> JuniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 1, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 1, "2Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 2, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(8, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(10, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> JuniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return await JuniorMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static async Task<ProgramComponentHandler> NoviceAMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 1, "1Eu"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 2, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(7, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(8, 0, "CoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(9, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> NoviceAWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return await NoviceAMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static async Task<ProgramComponentHandler> NoviceBMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHandler();
            await programComponentHanlder.Initialize(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            await programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(await programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(2, 0, "3A"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(3, 1, "3T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 1, "2Lo"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(4, 2, "2T"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(5, 0, "FCSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(6, 0, "SSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(7, 0, "CoSp4"));
                    Assert.IsTrue(await programComponentHanlder.TrySet(8, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static async Task<ProgramComponentHandler> NoviceBWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return await NoviceBMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }
    }
}
