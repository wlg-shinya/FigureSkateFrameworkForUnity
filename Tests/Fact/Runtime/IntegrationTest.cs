using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using Wlg.FigureSkate.Core;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact;
using Wlg.FigureSkate.Fact.Data;
using Assert = UnityEngine.Assertions.Assert;
using System.Linq;

namespace Wlg.FigureSkate.Tests.Fact
{
    public class IntegrationTest
    {
        [TestCase("NoviceA", "Men")]
        [TestCase("NoviceA", "Women")]
        [TestCase("NoviceB", "Men")]
        [TestCase("NoviceB", "Women")]
        public void AllJapanNoviceChampionship(string classId, string sexId) => Competition("AllJapanNoviceChampionship", new(2023, 10, 22), classId, sexId);
        [TestCase("Junior", "Men")]
        [TestCase("Junior", "Women")]
        public void ISUJuniorGrandPrixOsaka(string classId, string sexId) => Competition("ISUJuniorGrandPrixOsaka", new(2023, 9, 13), classId, sexId);
        [TestCase("Senior", "Men")]
        [TestCase("Senior", "Women")]
        public void KinoshitaGroupCupJapanOpen2023(string classId, string sexId) => Competition("KinoshitaGroupCupJapanOpen2023", new(2023, 10, 7), classId, sexId);

        private void Competition(string competitionName, YearMonthDay startDay, string classId, string sexId)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(startDay);

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
                foreach (var id in ElementObjectLoader.AllIds)
                {
                    player.elementIdList.Add(id);
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
            var classObjectAll = ClassObjectLoader.All(startDay);
            var competitionObjectAll = CompetitionObjectLoader.All(startDay);
            var sexObjectAll = SexObjectLoader.All();
            var className = ClassObjectQuery.ById(classObjectAll, player.classId).data.name;
            var sexName = SexObjectQuery.ById(sexObjectAll, player.sexId).data.name;
            Debug.Log($"{player.name}({YearMonthDayUtility.GetAge(startDay, player.birthday)}) / {className} / {sexName}");

            // 大会
            var competitionObjects = CompetitionObjectQuery.ByClassAndSex(competitionObjectAll, player.classId, player.sexId);
            var competitionObject = competitionObjects.Find(x => Equals(x.name, competitionName));
            Assert.IsNotNull(competitionObject);
            Debug.Log(competitionObject.data.name);
            Assert.AreEqual(competitionObject.data.startDay, startDay);

            // 大会中のイベントに合わせてプログラム構成を構築
            var elementObjectAll = ElementObjectLoader.All(startDay);
            var programObjects = ProgramObjectQuery.ByPlayerWithSetupConditions(competitionObject, player);
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
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementObjectAll, 0), 39.72f);
                        // 入れ替えによってジャンプボーナスが変わっているはず
                        programComponentHanlder.Swap(0, 2);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementObjectAll, 0), 39.7f);
                        // 入れ替えたものを元に戻す
                        programComponentHanlder.Swap(2, 0);
                        // GOE最大・最小の値が想定通りか
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementObjectAll, -5), 19.86f);
                        Assert.AreApproximatelyEqual(ProgramUtility.EstimateTotalBaseValue(programObject.data, player.programComponentsList[i].components, elementObjectAll, 5), 59.58f);
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
                Judge(programComponentHanlder.Program, programComponentHanlder.ProgramComponents, elementObjectAll);
            }
        }

        private void Judge(Program program, ProgramComponent[] programComponents, List<ElementObject> elementObjectAll)
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            int Random0To99() => UnityEngine.Random.Range(0, 100);

            var judge = new Judge(program, programComponents, elementObjectAll);
            judge.Execute(
                // GOE加点項目のチェックを通ったらtrueを返す
                (GoePlus goePlus, ElementObject elementObject) =>
                {
                    return true;
                },
                // GOE減点項目に引っかからなかったらtrueを返す
                (GoeMinus goeMinus, ElementObject elementObject) =>
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
