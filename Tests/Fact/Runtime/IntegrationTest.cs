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
        [TestCase("Senior", "Men")]
        [TestCase("Senior", "Women")]
        public async Task KinoshitaGroupCupJapanOpen2023(string classId, string sexId) => await Competition("KinoshitaGroupCupJapanOpen2023", new(2023, 10, 7), classId, sexId);

        private async Task Competition(string competitionName, YearMonthDay startDay, string classId, string sexId)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(startDay);
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
                foreach (var elementObject in elementObjectAll)
                {
                    player.elementIdList.Add(elementObject.data.id);
                }
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
            var classObjectAll = await ClassObjectQuery.All(startDay);
            var competitionObjectAll = await CompetitionObjectQuery.All(startDay);
            var elementBaseValueObjectAll = await ElementBaseValueObjectQuery.All(startDay);
            var goeObjectAll = await GoeObjectQuery.All(startDay);
            var sexObjectAll = await SexObjectQuery.All();
            var eventObjectAll = await EventObjectQuery.All(startDay);
            var className = ClassObjectQuery.ById(classObjectAll, player.classId).data.name;
            var sexName = SexObjectQuery.ById(sexObjectAll, player.sexId).data.name;
            Debug.Log($"{player.name}({YearMonthDayUtility.GetAge(startDay, player.birthday)}) / {className} / {sexName}");

            // 大会
            var competitionObjects = CompetitionObjectQuery.ByClassAndSex(competitionObjectAll, eventObjectAll, player.classId, player.sexId);
            var competitionObject = competitionObjects.Find(x => Equals(x.name, competitionName));
            Assert.IsNotNull(competitionObject);
            Debug.Log(competitionObject.data.name);
            Assert.AreEqual(competitionObject.data.startDay, startDay);

            // 大会中のイベントに合わせてプログラム構成を構築
            var programObjects = ProgramObjectQuery.ByPlayerWithSetupConditions(competitionObject, eventObjectAll, player);
            player.programComponentsList = new Player.ProgramComponents[programObjects.Count()];
            var programComponentHanlders = new List<ProgramComponentHanlder>();
            for (var i = 0; i < programObjects.Count(); i++)
            {
                var programObject = programObjects[i];
                player.programComponentsList[i] = new()
                {
                    components = ProgramComponentQuery.Create(programObject.data)
                };
                if (Equals(programObject.name, "SeniorMenShortProgram"))
                {
                    var programComponentHanlder = ProgramComponentHanlderFactory.SeniorMenShortProgram(programObject.data, player.programComponentsList[i].components);
                    programComponentHanlders.Add(programComponentHanlder);

                    // 合計基礎点テスト
                    {
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementBaseValueObjectAll, 0), 39.72f);
                        // 入れ替えによってジャンプボーナスが変わっているはず
                        programComponentHanlder.Swap(0, 2);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementBaseValueObjectAll, 0), 39.7f);
                        // 入れ替えたものを元に戻す
                        programComponentHanlder.Swap(2, 0);
                        // GOE最大・最小の値が想定通りか
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementBaseValueObjectAll, -5), 19.86f);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementBaseValueObjectAll, 5), 59.58f);
                    }
                }
                else if (Equals(programObject.name, "SeniorWomenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorWomenShortProgram(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "SeniorMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorMenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "SeniorWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.SeniorWomenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "JuniorMenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorMenShortProgram(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "JuniorWomenShortProgram"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorWomenShortProgram(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "JuniorMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorMenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "JuniorWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.JuniorWomenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "NoviceAMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceAMenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "NoviceAWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceAWomenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "NoviceBMenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceBMenFreeSkating(programObject.data, player.programComponentsList[i].components));
                }
                else if (Equals(programObject.name, "NoviceBWomenFreeSkating"))
                {
                    programComponentHanlders.Add(ProgramComponentHanlderFactory.NoviceBWomenFreeSkating(programObject.data, player.programComponentsList[i].components));
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
                    elementObjectAll.Select(x => x.data).ToList(),
                    elementBaseValueObjectAll.Select(x => x.data).ToList(),
                    goeObjectAll.Select(x => x.data).ToList()
                    );
            }
        }

        private void Judge(Program program, ProgramComponent[] programComponents, List<Element> elementAll, List<ElementBaseValue> elementBaseValueAll, List<Goe> goeAll)
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int Random0To99() => UnityEngine.Random.Range(0, 100);

            var judge = new Judge(program, programComponents, elementAll, elementBaseValueAll, goeAll);
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
