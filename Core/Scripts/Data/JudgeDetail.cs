using System;
using System.Collections.Generic;
using System.Linq;

namespace Wlg.FigureSkate.Core.Data
{
    // 判定の詳細データ
    // ユーザがデータを用意するのではなく、Judge クラスがデータを用意します
    [Serializable]
    public class JudgeDetail
    {
        public JudgeDetail(int programComponentLength)
        {
            tes = new Tes[programComponentLength];
            for (var i = 0; i < tes.Length; i++)
            {
                tes[i] = new Tes();
            }
            for (var i = 0; i < pcs.Length; i++)
            {
                pcs[i] = new Pcs()
                {
                    name = PcsName(i)
                };
            }
        }

        // 技術点
        [Serializable]
        public class Tes
        {
            // 実行済み要素名
            public string executedElement;
            // 転倒したか
            public bool fall;
            // 基礎点
            public float baseValue;
            // 後半ジャンプボーナス対象か
            public bool lastJump;
            // GOEスコア
            public float goeScore;
            // 審判ごとのGOE
            public int[] refereeGoe = new int[Constant.REFEREE_COUNT];

            // 要素ごとの情報記号
            [Serializable]
            public class InfoMarks
            {
                public List<string> marks = new();
                public string Unified() => InfoMarkListToString(marks);
            }
            public List<InfoMarks> infoMarksList = new();

            // この要素の最終スコア
            public float Score() => Round(baseValue + goeScore);
            // この要素の最終情報記号
            public string UnifiedInfoMark()
            {
                if (infoMarksList.Count() > 0)
                {
                    var uniqueMarks = infoMarksList
                        .SelectMany(x => x.marks) // 多重リストの平坦化
                        .Distinct(); // 重複削除
                    return InfoMarkListToString(uniqueMarks.ToList());
                }
                else
                {
                    return "";
                }
            }

            // 審判のGOEの平均値
            // 全審判の出した値のうち最大値と最低値を除いた平均値とする
            // ref. https://www.jsports.co.jp/skate/about/game/
            public float RefereeGoeAverage()
            {
                var max = refereeGoe.Max();
                var min = refereeGoe.Min();
                var maxRefereeIndex = refereeGoe.ToList().FindIndex(x => x == max);
                var minRefereeIndex = refereeGoe.ToList().FindIndex(x => x == min);
                var result = (float)refereeGoe
                    .Where((x, i) => i != maxRefereeIndex && i != minRefereeIndex)
                    .Average();
                return Round(result);
            }

            public Tes DeepCopy()
            {
                var newTes = new Tes
                {
                    executedElement = executedElement.Clone() as string,
                    fall = fall,
                    baseValue = baseValue,
                    lastJump = lastJump,
                    goeScore = goeScore,
                };
                for (var i = 0; i < refereeGoe.Length; i++) newTes.refereeGoe[i] = refereeGoe[i];
                foreach (var infoMarks in infoMarksList)
                {
                    var newInfoMarks = new InfoMarks();
                    foreach (var mark in infoMarks.marks) newInfoMarks.marks.Add(mark.Clone() as string);
                    newTes.infoMarksList.Add(newInfoMarks);
                }
                return newTes;
            }

            // 記号群を文字列化
            // 順序は国際基準( https://current.isu.org/figure-skating/rules/fsk-communications )に準拠
            static private string InfoMarkListToString(List<string> marks)
            {
                string resultMarks = "";
                if (marks.Any(x => x.Equals("e"))) resultMarks += "e";
                else if (marks.Any(x => x.Equals("!"))) resultMarks += "!";
                if (marks.Any(x => x.Equals("<<"))) resultMarks += "<<";
                else if (marks.Any(x => x.Equals("<"))) resultMarks += "<";
                else if (marks.Any(x => x.Equals("q"))) resultMarks += "q";
                if (marks.Any(x => x.Equals("V"))) resultMarks += "V";
                return resultMarks;
            }
        }
        // 演技構成点
        [Serializable]
        public class Pcs
        {
            // 項目名
            public string name;
            // 倍率
            public float factor;
            // 審判ごとのスコア
            public float[] refereeScore = new float[Constant.REFEREE_COUNT];

            // 審判のスコアの平均値
            // 全審判の出した値のうち最大値と最低値を除いた平均値とする
            // ref. https://www.jsports.co.jp/skate/about/game/
            public float RefereeScoreAverage()
            {
                var max = refereeScore.Max();
                var min = refereeScore.Min();
                var maxRefereeIndex = refereeScore.ToList().FindIndex(x => x == max);
                var minRefereeIndex = refereeScore.ToList().FindIndex(x => x == min);
                var result = refereeScore
                    .Where((x, i) => i != maxRefereeIndex && i != minRefereeIndex)
                    .Average();
                return Round(result);
            }

            public Pcs DeepCopy()
            {
                var newPcs = new Pcs
                {
                    name = name,
                    factor = factor,
                };
                for (var i = 0; i < refereeScore.Length; i++) newPcs.refereeScore[i] = refereeScore[i];
                return newPcs;
            }
        }
        public Tes[] tes;
        public Pcs[] pcs = new Pcs[Constant.PCS_COUNT];

        public float TotalBaseValue() => Round(tes.Sum(x => x.baseValue));
        public float TotalElementScore() => Round(tes.Sum(x => x.Score()));
        public float TotalProgramComponentScore() => Round(pcs.Sum(x => x.RefereeScoreAverage()) * pcs.Average(x => x.factor));
        public float TotalSegmentScore() => Round(TotalElementScore() + TotalProgramComponentScore());
        // TODO:Deduction 対応
        public float TotalDeductions() => Round(0.0f);

        public JudgeDetail DeepCopy()
        {
            var newJudgeDetail = new JudgeDetail(tes.Length);
            for (var i = 0; i < tes.Length; i++) newJudgeDetail.tes[i] = tes[i].DeepCopy();
            for (var i = 0; i < pcs.Length; i++) newJudgeDetail.pcs[i] = pcs[i].DeepCopy();
            return newJudgeDetail;
        }

        // 情報出力
        public string Dump()
        {
            var separator = "\n----\n";
            var referees = "J1,J2,J3,J4,J5,J6,J7,J8,J9";
            var s = $"#,Executed Elements,Info,Fall,Base Value,,GOE,{referees},Ref Scores Of Panel\n";
            // TES
            s += tes
                .Select((x, i) =>
                {
                    var fallMark = x.fall ? "F" : "";
                    var lastJumpMark = x.lastJump ? "X" : "";
                    var refereeGoes = x.refereeGoe.Aggregate("", (a, c) => Equals(a, "") ? $"{c}" : $"{a},{c}");
                    return $"{i + 1},{x.executedElement},{x.UnifiedInfoMark()},{fallMark},{x.baseValue},{lastJumpMark},{x.goeScore},{refereeGoes},{x.Score()}";
                })
                .Aggregate((a, c) => a + "\n" + c);
            s += separator;
            // PCS
            s += $"Program Components,Factor,{referees},Ref Scores Of Panel\n";
            s += pcs
                .Select(x =>
                {
                    var refereeScores = x.refereeScore.Aggregate("", (a, c) => Equals(a, "") ? $"{c}" : $"{a},{c}");
                    return $"{x.name},{x.factor},{refereeScores},{x.RefereeScoreAverage()}";
                })
                .Aggregate((a, c) => a + "\n" + c);
            s += separator;
            // Total
            s += "Total Base Value,Total Element Score,Total Program Component Score,Total Segment Score\n";
            s += $"{TotalBaseValue()},{TotalElementScore()},{TotalProgramComponentScore()},{TotalSegmentScore()}";
            return s;
        }

        // PCSの項目番号ごとの名前
        private string PcsName(int index)
        {
            return index switch
            {
                0 => "Composition",
                1 => "Presentation",
                2 => "Skating Skills",
                _ => throw new ArgumentOutOfRangeException($"index = {index}"),
            };
        }

        // 丸め。スコアの最終表示に対して実行するべき処理
        // 計算中は小数点をそのまま扱い最終的な表示時に小数点第二位で四捨五入としている模様
        // ref. https://www.isu.org/figure-skating/rules/fsk-regulations-rules/file
        // ref. https://results.isu.org/results/season2223/wtt2023/data0203.pdf
        static private float Round(float value) => (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
    }
}