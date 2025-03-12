using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Fact;
using Wlg.FigureSkate.Fact.Data;
using Assert = UnityEngine.Assertions.Assert;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class IntegrationTest
    {
        // 2023-24シーズン大会のテスト
        [TestCase("KinoshitaGroupCupJapanOpen2023", "2023/10/7", "Senior", "Men")]
        [TestCase("KinoshitaGroupCupJapanOpen2023", "2023/10/7", "Senior", "Women")]
        [TestCase("ISUWorldJuniorChampionships2024", "2024/2/26", "Junior", "Men")]
        [TestCase("ISUWorldJuniorChampionships2024", "2024/3/3", "Junior", "Women")]
        [TestCase("NationalNovice", "2023/10/20", "NoviceA", "Men")]
        [TestCase("NationalNovice", "2023/10/21", "NoviceA", "Women")]
        [TestCase("NationalNovice", "2023/10/22", "NoviceB", "Men")]
        [TestCase("NationalNovice", "2023/10/22", "NoviceB", "Women")]
        // 2024-25シーズン大会のテスト
        [TestCase("ISUGPNHKTrophy2024", "2024/11/8", "Senior", "Men")]
        [TestCase("ISUGPNHKTrophy2024", "2024/11/10", "Senior", "Women")]
        [TestCase("ISUWorldJuniorFigureSkatingChampionships2025", "2025/2/25", "Junior", "Men")]
        [TestCase("ISUWorldJuniorFigureSkatingChampionships2025", "2025/3/2", "Junior", "Women")]
        [TestCase("NationalNovice", "2024/10/18", "NoviceA", "Men")]
        [TestCase("NationalNovice", "2024/10/19", "NoviceA", "Women")]
        [TestCase("NationalNovice", "2024/10/20", "NoviceB", "Men")]
        [TestCase("NationalNovice", "2024/10/20", "NoviceB", "Women")]
        public async Task Competition(string competitionName, string basedayString, string classId, string sexId)
        {
            var baseday = new YearMonthDay(basedayString);
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            var elementObjectAll = await ElementObjectQuery.All();

            // プレイヤー
            Player CreatePlayer(
                string name,
                string nameOnScore,
                YearMonthDay birthday,
                string sexId,
                string classId,
                string nation)
            {
                Player player = new()
                {
                    name = name,
                    nameOnScore = nameOnScore,
                    birthday = birthday,
                    sexId = sexId,
                    classId = classId,
                    nation = nation
                };
                // 全構成要素を習得済みとする
                player.elementIdList = elementObjectAll.Select(x => x.data.id).ToArray();
                return player;
            }
            Player player;
            if (Equals(sexId, "Men"))
            {
                if (Equals(classId, "Senior"))
                {
                    player = CreatePlayer("赤砂たな", "Tana AKASA", new(2000, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "Junior"))
                {
                    player = CreatePlayer("壱岐しちに", "Shichini IKI", new(2010, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "NoviceA"))
                {
                    player = CreatePlayer("宇久すつぬ", "Sutsunu UKU", new(2012, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "NoviceB"))
                {
                    player = CreatePlayer("江家せてね", "Setene EKE", new(2014, 1, 1), sexId, classId, "JPN");
                }
                else throw new ArgumentException($"classId = {classId}");
            }
            else if (Equals(sexId, "Women"))
            {
                if (Equals(classId, "Senior"))
                {
                    player = CreatePlayer("波間やらわ", "Yarawa HAMA", new(2000, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "Junior"))
                {
                    player = player = CreatePlayer("氷見いりい", "Irii HIMI", new(2010, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "NoviceA"))
                {
                    player = CreatePlayer("不無ゆるう", "Yuruu FUMU", new(2012, 1, 1), sexId, classId, "JPN");
                }
                else if (Equals(classId, "NoviceB"))
                {
                    player = CreatePlayer("経目えれえ", "Eree HEME", new(2014, 1, 1), sexId, classId, "JPN");
                }
                else throw new ArgumentException($"classId = {classId}");
            }
            else throw new ArgumentException($"sexId = {sexId}");
            var classObjectAll = await ClassObjectQuery.All(baseday);
            var eventObjectAll = await EventObjectQuery.All();
            var programObjectAll = await ProgramObjectQuery.All(baseday);
            var programComponentRegulationObjectAll = await ProgramComponentRegulationObjectQuery.All(baseday);
            var elementPlaceableSetObjectAll = await ElementPlaceableSetObjectQuery.All();
            var elementPlaceableObjectAll = await ElementPlaceableObjectQuery.All();
            var competitionObjectAll = await CompetitionObjectQuery.All(baseday);
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(baseday);
            var goeObjectAll = await GoeObjectQuery.All(baseday);
            var goePlusObjectAll = await GoePlusObjectQuery.All(baseday);
            var goeMinusObjectAll = await GoeMinusObjectQuery.All(baseday);
            var sexObjectAll = await SexObjectQuery.All();
            var className = ClassObjectQuery.ById(classObjectAll, player.classId).data.name;
            var sexName = SexObjectQuery.ById(sexObjectAll, player.sexId).data.name;
            Debug.Log($"{player.name}({YearMonthDayUtility.GetAge(baseday, player.birthday)}) / {className} / {sexName}");

            // 大会
            var competitionObjects = CompetitionObjectQuery.ByClassAndSex(competitionObjectAll, eventObjectAll, player.classId, player.sexId);
            var competitionObject = competitionObjects.Find(x => Equals(x.name, competitionName));
            Assert.IsNotNull(competitionObject);
            Debug.Log(competitionObject.data.name);
            var isCompetitionPeriod = competitionObject.data.startDay <= baseday && baseday <= competitionObject.data.endDay;
            Assert.IsTrue(isCompetitionPeriod);

            // 大会中のイベントに合わせてプログラム構成を構築
            var programObjects = ProgramObjectQuery.ByPlayerWithSetupConditions(competitionObject, eventObjectAll, programObjectAll, programComponentRegulationObjectAll, elementPlaceableSetObjectAll, player);
            player.programComponentsList = new Player.ProgramComponents[programObjects.Count()];
            var programComponentHanlders = new List<ProgramComponentHanlder>();

            var programComponentRegulationAll = programComponentRegulationObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableSetAll = elementPlaceableSetObjectAll.Select(x => x.data).ToArray();
            var elementPlaceableAll = elementPlaceableObjectAll.Select(x => x.data).ToArray();
            var elementBaseValueAll = elementBaseValueObjectAll.Select(x => x.data).ToArray();
            for (var i = 0; i < programObjects.Count(); i++)
            {
                var programObject = programObjects[i];
                var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjectAll, programObject.data.programComponentRegulationId);
                player.programComponentsList[i] = new()
                {
                    components = ProgramComponentQuery.Create(regulation.data, elementPlaceableSetObjectAll)
                };
                if (Equals(programObject.name, "SeniorMenShortProgram"))
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll);
                    programComponentHanlders.Add(programComponentHanlder);

                    // 合計基礎点テスト
                    {
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementPlaceableSetAll, elementBaseValueAll, 0), 39.72f);
                        // 入れ替えによってジャンプボーナスが変わっているはず
                        programComponentHanlder.Swap(0, 2);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementPlaceableSetAll, elementBaseValueAll, 0), 39.7f);
                        // 入れ替えたものを元に戻す
                        programComponentHanlder.Swap(2, 0);
                        // GOE最大・最小の値が想定通りか
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementPlaceableSetAll, elementBaseValueAll, -5), 19.86f);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementPlaceableSetAll, elementBaseValueAll, 5), 59.58f);
                    }
                }
                else if (Equals(programObject.name, "SeniorWomenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "SeniorMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorMenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "SeniorWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorWomenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "JuniorMenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "JuniorWomenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "JuniorMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorMenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "JuniorWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorWomenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "NoviceAMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "NoviceAWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "NoviceBMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else if (Equals(programObject.name, "NoviceBWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, player.programComponentsList[i].components, skateYear, programComponentRegulationAll, elementPlaceableSetAll, elementPlaceableAll));
                }
                else throw new Exception($"Invalid ProgramObject.name = {programObject.name}");
            }

            // ジャッジ
            foreach (var programComponentHanlder in programComponentHanlders)
            {
                Debug.Log(programComponentHanlder.Program.name);
                Judge(
                    programComponentHanlder.Program,
                    programComponentHanlder.ProgramComponents,
                    elementObjectAll.Select(x => x.data).ToArray(),
                    elementBaseValueAll,
                    goeObjectAll.Select(x => x.data).ToArray(),
                    goePlusObjectAll.Select(x => x.data).ToArray(),
                    goeMinusObjectAll.Select(x => x.data).ToArray(),
                    programComponentRegulationAll,
                    elementPlaceableSetAll
                    );
            }
        }

        private void Judge(
            Program program,
            ProgramComponent[] programComponents,
            Element[] elementAll,
            ElementBaseValue[] elementBaseValueAll,
            Goe[] goeAll,
            GoePlus[] goePlusAll,
            GoeMinus[] goeMinusAll,
            ProgramComponentRegulation[] programComponentRegulationAll,
            ElementPlaceableSet[] elementPlaceableSetAll
            )
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int Random0To99() => UnityEngine.Random.Range(0, 100);

            var judge = new Judge(program, programComponents, elementAll, elementBaseValueAll, goeAll, goePlusAll, goeMinusAll, programComponentRegulationAll, elementPlaceableSetAll);
            judge.Execute(
                // GOE加点項目のチェックを通ったらtrueを返す
                (GoePlus goePlus, Element element) =>
                {
                    return true;
                },
                // GOE減点項目に引っかからなかったらtrueを返す
                (GoeMinus goeMinus, Element element) =>
                {
                    return Random0To99() < 50;
                },
                // GOE減点項目の減点量(GoeMinus.minValue-GoeMinus.maxValue)を返す
                (GoeMinus goeMinus) =>
                {
                    return goeMinus.minValue;
                },
                // ジャンプが成功したらtrueを返す
                () =>
                {
                    var JUMP_SUCCESS_POINT = 95; // = 95%
                    return Random0To99() < JUMP_SUCCESS_POINT;
                },
                // 構成力をチェックし出来栄え(0-1。大きいほど良い)を返す
                () =>
                {
                    return Random0To99() / 99.0f;
                },
                // 演技力をチェックし出来栄え(0-1。大きいほど良い)を返す
                () =>
                {
                    return Random0To99() / 99.0f;
                },
                // スケート技術をチェックし出来栄え(0-1。大きいほど良い)を返す
                () =>
                {
                    return Random0To99() / 99.0f;
                });
            Debug.Log(judge.Detail.Dump());
        }
    }
}
