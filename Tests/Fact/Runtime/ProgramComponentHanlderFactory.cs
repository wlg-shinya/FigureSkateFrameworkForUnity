using Wlg.FigureSkate.Core;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public static class ProgramComponentHanlderFactory
    {
        public static ProgramComponentHanlder SeniorMenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorWomenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(10, 0, "StSqB"));
                    Assert.IsTrue(programComponentHanlder.TrySet(11, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return SeniorMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static ProgramComponentHanlder JuniorMenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2023-24":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2024-25":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorWomenShortProgram(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2023-24":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2024-25":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "4Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 1, "2Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(10, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return JuniorMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static ProgramComponentHanlder NoviceAMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "1Eu"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder NoviceAWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return NoviceAMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static ProgramComponentHanlder NoviceBMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            programComponentHanlder.UnsetAll();
            switch (skateYear)
            {
                case "2022-23":
                case "2023-24":
                case "2024-25":
                case "2025-26":
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder NoviceBWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            string skateYear,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return NoviceBMenFreeSkating(program, programComponents, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }
    }
}
