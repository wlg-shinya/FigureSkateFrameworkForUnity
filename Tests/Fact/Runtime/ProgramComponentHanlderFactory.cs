using System.Collections.Generic;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Core.Data;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public static class ProgramComponentHanlderFactory
    {
        // ここで ProgramComponentHanlder クラス自体のテストも兼ねる
        public static ProgramComponentHanlder SeniorMenShortProgram2023_24(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
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
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3回転+2回転は構成できません"
                {
                    programComponentHanlder.TrySet(2, 0, "3A");
                    Assert.IsFalse(programComponentHanlder.TrySet(2, 1, "2Lo"));
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(0, 0, "3A");
                    programComponentHanlder.TrySet(2, 0, "3A");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(2, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(3, 0, "FSSp4");
                    programComponentHanlder.TrySet(4, 0, "CSSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(3, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(6, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorWomenShortProgram2023_24(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
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
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "同じジャンプを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(0, 0, "3A");
                    programComponentHanlder.TrySet(2, 0, "3A");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(2, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(3, 0, "FSSp4");
                    programComponentHanlder.TrySet(4, 0, "SSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(3, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(6, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
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
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(10, 0, "StSqB"));
                    Assert.IsTrue(programComponentHanlder.TrySet(11, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
                    programComponentHanlder.TrySet(6, 0, "3S");
                    programComponentHanlder.TrySet(6, 1, "2Lo");
                    Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "2A"));
                    // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                    programComponentHanlder.TrySet(6, 1, "1Eu");
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                    // もともと構成できない要素は当然構成できない
                    Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "アクセルジャンプを少なくともひとつ構成する必要があります"
                {
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "2S"));
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(6, 0, "3S");
                    programComponentHanlder.TrySet(6, 1, "1Eu");
                    programComponentHanlder.TrySet(6, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(0, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(6, 0, "3T");
                    programComponentHanlder.TrySet(6, 1, "3T");
                    programComponentHanlder.TrySet(6, 2, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(0, 0, "4T");
                    programComponentHanlder.TrySet(2, 0, "4T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じスピンを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(8, 0, "FCoSp4");
                    programComponentHanlder.TrySet(9, 0, "FCoSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(11, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder SeniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return SeniorMenFreeSkating(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static ProgramComponentHanlder JuniorMenShortProgram2023_24(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "FSSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "CCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "CCoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "同じジャンプを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(1, 0, "3Lz");
                    programComponentHanlder.TrySet(2, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(2, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(6, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorWomenShortProgram2023_24(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
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
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "同じジャンプを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(1, 0, "3Lz");
                    programComponentHanlder.TrySet(2, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 同じ要素でもジャンプコンビネーションならエラーにはならない
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(2, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(6, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
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
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(10, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
                    programComponentHanlder.TrySet(6, 0, "3S");
                    programComponentHanlder.TrySet(6, 1, "2Lo");
                    Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "2A"));
                    // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                    programComponentHanlder.TrySet(6, 1, "1Eu");
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 2, "2A"));
                    // もともと構成できない要素は当然構成できない
                    Assert.IsFalse(programComponentHanlder.TrySet(6, 2, "ChSq1"));
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "アクセルジャンプを少なくともひとつ構成する必要があります"
                {
                    programComponentHanlder.TrySet(1, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(6, 0, "3S");
                    programComponentHanlder.TrySet(6, 1, "1Eu");
                    programComponentHanlder.TrySet(6, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(0, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(6, 0, "3T");
                    programComponentHanlder.TrySet(6, 1, "3T");
                    programComponentHanlder.TrySet(6, 2, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
                    // 3回転は3種類以上2回繰り返すことはできない
                    programComponentHanlder.TrySet(2, 0, "3F");
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(3, 0, "3S");
                    programComponentHanlder.TrySet(6, 0, "3S");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(0, 0, "4T");
                    programComponentHanlder.TrySet(2, 0, "4T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じスピンを複数構成することはできません"
                {
                    programComponentHanlder.TrySet(8, 0, "FCoSp4");
                    programComponentHanlder.TrySet(9, 0, "FCoSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(10, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder JuniorWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            return JuniorMenFreeSkating(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
        }

        public static ProgramComponentHanlder NoviceAMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "1Eu"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
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
                    programComponentHanlder.TrySet(5, 2, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(5, 1, "1Eu");
                    programComponentHanlder.TrySet(5, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "1T");
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 2, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4Lo");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "第1ジャンプにループ、ルッツ、フリップの3種類を含めてください"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(1, 0, "3T");
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(6, 0, "SSp4");
                    programComponentHanlder.TrySet(7, 0, "FSSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(7, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(9, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder NoviceAWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "3Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 1, "1Eu"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 2, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(9, 0, "StSqB"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
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
                    programComponentHanlder.TrySet(5, 2, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(5, 0, "3F");
                    programComponentHanlder.TrySet(5, 1, "1Eu");
                    programComponentHanlder.TrySet(5, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "1T");
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 単独含めて全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(5, 0, "3T");
                    programComponentHanlder.TrySet(5, 2, "3T");
                    programComponentHanlder.TrySet(5, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    programComponentHanlder.TrySet(2, 0, "3Lo");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(4, 0, "3Lz");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(5, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(2, 0, "4Lo");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "第1ジャンプにループ、ルッツ、フリップの3種類を含めてください"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(1, 0, "3T");
                    programComponentHanlder.TrySet(3, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(6, 0, "SSp4");
                    programComponentHanlder.TrySet(7, 0, "FSSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(7, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(9, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder NoviceBMenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(4, 1, "2Lo");
                    programComponentHanlder.TrySet(4, 2, "2T");
                    Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "2A"));
                    // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                    programComponentHanlder.TrySet(4, 1, "1Eu");
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                    // もともと構成できない要素は当然構成できない
                    Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "ChSq1"));
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "アクセルジャンプを少なくともひとつ構成する必要があります"
                {
                    programComponentHanlder.TrySet(2, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(4, 1, "1Eu");
                    programComponentHanlder.TrySet(4, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "3T");
                    programComponentHanlder.TrySet(3, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 2, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    programComponentHanlder.TrySet(4, 2, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "第1ジャンプにルッツ、フリップの2種類を含めてください"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    programComponentHanlder.TrySet(0, 0, "3F");
                    programComponentHanlder.TrySet(1, 0, "3F");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(5, 0, "SSp4");
                    programComponentHanlder.TrySet(6, 0, "FSSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(6, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(8, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }

        public static ProgramComponentHanlder NoviceBWomenFreeSkating(
            Program program,
            ProgramComponent[] programComponents,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll,
            ElementPlaceable[] elementPlaceableAll
            )
        {
            // プログラム構成
            var programComponentHanlder = new ProgramComponentHanlder(program, programComponents, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
            {
                // 構成セット
                void SetupValidProgramComponent(ProgramComponentHanlder programComponentHanlder)
                {
                    programComponentHanlder.UnsetAll();
                    Assert.IsTrue(programComponentHanlder.TrySet(0, 0, "3Lz"));
                    Assert.IsTrue(programComponentHanlder.TrySet(1, 0, "3F"));
                    Assert.IsTrue(programComponentHanlder.TrySet(2, 0, "2A"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 0, "3S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(3, 1, "3T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 0, "2S"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 1, "2Lo"));
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2T"));
                    Assert.IsTrue(programComponentHanlder.TrySet(5, 0, "SSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(6, 0, "FCSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(7, 0, "CoSp4"));
                    Assert.IsTrue(programComponentHanlder.TrySet(8, 0, "ChSq1"));
                    // 正常に構成できているのでエラーは発生していないはず
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                }
                SetupValidProgramComponent(programComponentHanlder);

                // "3連続ジャンプコンビネーションは2番目にオイラーを構成しないと3番目のジャンプはトウループかループしか構成できません"
                {
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(4, 1, "2Lo");
                    programComponentHanlder.TrySet(4, 2, "2T");
                    Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "2A"));
                    // ただし2番目に1Euを配置した場合に限り上記制限が解除される
                    programComponentHanlder.TrySet(4, 1, "1Eu");
                    Assert.IsTrue(programComponentHanlder.TrySet(4, 2, "2A"));
                    // もともと構成できない要素は当然構成できない
                    Assert.IsFalse(programComponentHanlder.TrySet(4, 2, "ChSq1"));
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "アクセルジャンプを少なくともひとつ構成する必要があります"
                {
                    programComponentHanlder.TrySet(2, 0, "2S");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // コンビネーション中でもアクセルを含んでいるならOK
                    programComponentHanlder.TrySet(4, 0, "2S");
                    programComponentHanlder.TrySet(4, 1, "1Eu");
                    programComponentHanlder.TrySet(4, 2, "2A");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "同じジャンプは3回以上構成できません"
                {
                    // どのような回転数でもダメ
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 2連続コンビネーションで連続構成はOK
                    programComponentHanlder.TrySet(3, 0, "3T");
                    programComponentHanlder.TrySet(3, 1, "3T");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    // 全体で3回以上の構成はダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 3連続コンビネーション中でもダメ
                    programComponentHanlder.TrySet(4, 0, "3T");
                    programComponentHanlder.TrySet(4, 2, "3T");
                    programComponentHanlder.TrySet(4, 1, "3T");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "すべての3回転および4回転ジャンプの中から2種類のみ2回繰り返すことができます。2種類の繰り返しのうち4回転は1種類のみ認められます"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2回繰り返す場合、3回転は1種類しか繰り返すことができない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    programComponentHanlder.TrySet(0, 0, "3Lz");
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "3Lo");
                    programComponentHanlder.TrySet(4, 2, "3Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                    // 4回転を2種類2回繰り返すことはできない
                    programComponentHanlder.TrySet(1, 0, "4F");
                    programComponentHanlder.TrySet(4, 0, "4F");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    programComponentHanlder.TrySet(4, 1, "4Lo");
                    programComponentHanlder.TrySet(4, 2, "4Lo");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "第1ジャンプにルッツ、フリップの2種類を含めてください"
                {
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
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "単独ジャンプでは同じジャンプを複数構成すると基礎点が減るので控えてください"
                {
                    programComponentHanlder.TrySet(3, 0, "3Lz");
                    programComponentHanlder.TrySet(0, 0, "3F");
                    programComponentHanlder.TrySet(1, 0, "3F");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "フライングスピンと単一姿勢スピンでは異なる着氷姿勢を設定してください"
                {
                    programComponentHanlder.TrySet(5, 0, "SSp4");
                    programComponentHanlder.TrySet(6, 0, "FSSp4");
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    // 異なる着氷姿勢に設定しなおしたのでエラーは解消しているはず
                    programComponentHanlder.TrySet(6, 0, "FCSp4");
                    Assert.AreEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }

                // "構成要素はすべて設定してください"
                {
                    programComponentHanlder.Unset(8, 0);
                    Assert.AreNotEqual(programComponentHanlder.ErrorMessage, "");
                    SetupValidProgramComponent(programComponentHanlder);
                }
            }
            return programComponentHanlder;
        }
    }
}
