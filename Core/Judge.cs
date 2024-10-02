using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Core
{
    // ジャッジ。判定をしてスコアを出す
    // MEMO:
    // - +REPは非対応（プログラム構成条件で設定できないようにする想定）
    // - Fall判定はジャンプ単位ではなく構成単位で行っている。そのためジャンプコンビネーションは全部実行される
    public class Judge
    {
        public Judge(Program program, ProgramComponent[] programComponents, List<ElementObject> elementObjectAll)
        {
            _program = program;
            _programComponents = programComponents;
            _elementObjectAll = elementObjectAll;

            // 判定の詳細データの初期化
            var componentLength = _programComponents.Length;
            Detail = new JudgeDetail(componentLength);

            // GOE減点項目の初期化
            _goeMinus = new List<GoeMinus>[componentLength][][];
            for (var ProgramComponentIndex = 0; ProgramComponentIndex < componentLength; ProgramComponentIndex++)
            {
                var component = _programComponents[ProgramComponentIndex];
                _goeMinus[ProgramComponentIndex] = new List<GoeMinus>[Constant.REFEREE_COUNT][];
                for (var refereeIndex = 0; refereeIndex < Constant.REFEREE_COUNT; refereeIndex++)
                {
                    _goeMinus[ProgramComponentIndex][refereeIndex] = new List<GoeMinus>[component.elementIds.Length];
                    for (var elementIndex = 0; elementIndex < component.elementIds.Length; elementIndex++)
                    {
                        _goeMinus[ProgramComponentIndex][refereeIndex][elementIndex] = new List<GoeMinus>();
                    }
                }
            }
        }

        // GOE加点項目のチェックを通ったらtrueを返すデリゲート。
        public delegate bool SuccessGoePlus(GoePlus goePlus, ElementObject elementObject);
        // GOE減点項目に引っかからなかったらtrueを返すデリゲート。
        public delegate bool SuccessGoeMinus(GoeMinus goeMinus, ElementObject elementObject);
        // GOE減点項目の減点量(GoeMinus.minValue-GoeMinus.maxValue)を返すデリゲート。
        public delegate int CheckGoeMinusValue(GoeMinus goeMinus);
        // ジャンプが成功したらtrueを返すデリゲート
        public delegate bool SuccessJump();
        // 構成力をチェックし出来栄え(0-1。大きいほど良い)を返すデリゲート
        public delegate float CheckComposition();
        // 演技力をチェックし出来栄え(0-1。大きいほど良い)を返すデリゲート
        public delegate float CheckPresentation();
        // スケート技術をチェックし出来栄え(0-1。大きいほど良い)を返すデリゲート
        public delegate float CheckSkatingSkills();

        // 判定結果の詳細データ
        public JudgeDetail Detail { get; private set; }

        // 実行
        public void Execute(
            SuccessGoePlus successGoePlus,
            SuccessGoeMinus successGoeMinus,
            CheckGoeMinusValue checkGoeMinusValue,
            SuccessJump successJump,
            CheckComposition checkComposition,
            CheckPresentation checkPresentation,
            CheckSkatingSkills checkSkatingSkills)
        {
            CheckTes(successGoePlus, successGoeMinus, checkGoeMinusValue, successJump);
            CheckPcs(checkComposition, checkPresentation, checkSkatingSkills);
            CheckDeduction();
        }

        // 技術点の判定
        private void CheckTes(
            SuccessGoePlus successGoePlus,
            SuccessGoeMinus successGoeMinus,
            CheckGoeMinusValue checkGoeMinusValue,
            SuccessJump successJump)
        {
            // プログラム構成ごとに判定
            var components = _programComponents;
            for (var ProgramComponentIndex = 0; ProgramComponentIndex < components.Length; ProgramComponentIndex++)
            {
                var tes = Detail.tes[ProgramComponentIndex];
                var component = components[ProgramComponentIndex];
                var goeMinus = _goeMinus[ProgramComponentIndex];

                // 審判ごとのGOEを判定
                CheckTesRefereeGoe(tes, component, goeMinus, successGoePlus, successGoeMinus, checkGoeMinusValue);
                if (_program.regulation.elementPlaceableSets[ProgramComponentIndex].jump)
                {
                    // ジャンプ専用の転倒判定
                    // 転倒した場合は審判GOEも更新するのでこれ以降に審判GOEを変えると転倒ペナルティが正常に反映されないので注意
                    CheckTesJump(tes, successJump);
                }
                // 今回の結果を記録
                RecordTes(tes, component, goeMinus);
            }
        }

        // 審判ごとのGOEを判定
        // 全ての減点項目(GoeMinus)の生成も行う
        private void CheckTesRefereeGoe(
            JudgeDetail.Tes tes,
            ProgramComponent component,
            List<GoeMinus>[][] goeMinus,
            SuccessGoePlus successGoePlus,
            SuccessGoeMinus successGoeMinus,
            CheckGoeMinusValue checkGoeMinusValue)
        {
            for (var refereeIndex = 0; refereeIndex < Constant.REFEREE_COUNT; refereeIndex++)
            {
                tes.refereeGoe[refereeIndex] = component.elementIds
                    .Select((elementId, index) => (elementId, index))
                    .Min((data) => // 連続で要素を実行する場合のGOEは最も低い値を採用する
                    {
                        // 構成要素のデータからオブジェクトを取得
                        var elementObject = _elementObjectAll.Find(x => Equals(x.data.id, data.elementId)) ?? throw new Exception($"Not found '{data.elementId}'");
                        // 構成要素オブジェクトを用いてGOE判定を行う
                        var goePlusValue = CheckTesGoePlusValue(elementObject, successGoePlus);
                        var goeMinusValue = CheckTesGoeMinusValue(elementObject, goeMinus[refereeIndex][data.index], successGoeMinus, checkGoeMinusValue);
                        // すべての判定が終わったらこれまでのGOEの加減点を合算する
                        return Math.Clamp(goePlusValue + goeMinusValue, Constant.GOE_MIN_VALUE, Constant.GOE_MAX_VALUE);
                    });
            }
        }

        // GOE加点値の判定
        private int CheckTesGoePlusValue(ElementObject elementObject, SuccessGoePlus successGoePlus)
        {
            var plus = elementObject.goeObject.data.plus;

            // 成否判定
            var result = plus.Select(x => successGoePlus(x, elementObject));

            // GOEの重要項目数と実際に成功した重要項目数が一致していたら、全体の達成項目数かGOE最大値の低いほうをGOE加点として採用
            // そうでなければ全体の達成項目数かGOEの重要項目数の低いほうをGOE加点として採用
            // ref. https://www.jsports.co.jp/skate/about/game/
            var requiredCount = plus.Count((x) => x.important == true);
            var requiredSuccessCount = result.Where((x, i) => x && plus[i].important).Count();
            var successCount = result.Count((x) => x == true);
            return requiredCount == requiredSuccessCount ? Math.Min(successCount, Constant.GOE_MAX_VALUE) : Math.Min(successCount, requiredCount);
        }

        // GEO減点値の判定
        // 減点項目(GoeMinus)の生成も行う
        private int CheckTesGoeMinusValue(ElementObject elementObject, List<GoeMinus> goeMinus, SuccessGoeMinus successGoeMinus, CheckGoeMinusValue checkGoeMinusValue)
        {
            var minus = elementObject.goeObject.data.minus;

            // 成否判定を行いチェックに引っかかった減点項目の一覧を得る
            var minusFails = minus
                // 対象構成要素のみに絞る
                .Where((x) => x.targetElementIds.Length <= 0 || x.targetElementIds.Any(x => Equals(x, elementObject.data.id)))
                // ダウングレードしようがない要素はダウングレード判定を除外
                .Where((x) => !(!elementObject.downgrade && x.isDowngrade))
                // 成否判定。失敗したものだけ残す
                .Where((x) => !successGoeMinus(x, elementObject));

            if (minusFails.Count() > 0)
            {
                // 同グループの場合は代表を選出しつつ一つのリストにする
                var minusResult = minusFails
                    .GroupBy((x) => x.group)
                    .SelectMany((x) =>
                    {
                        if (Equals(x.Key, ""))
                        {
                            // グループ名が未設定のものは全部そのまま扱う
                            return x.ToArray();
                        }
                        else
                        {
                            // グループ内で最もGOE減点量が多い項目を選出
                            return new GoeMinus[] { x.Aggregate((a, c) => a.TotalValue() < c.TotalValue() ? a : c) };
                        }
                    });

                // GOE減点を合算
                var total = minusResult.Sum(x =>
                {
                    var value = checkGoeMinusValue(x);
                    if (x.minValue < value || value < x.maxValue)
                    {
                        throw new ArgumentOutOfRangeException($"value = {value}");
                    }
                    return value;
                });

                // 最終的な減点項目を登録
                foreach (var x in minusResult)
                {
                    goeMinus.Add(x);
                }

                // GOE減点、合算値か最低値の大きいほうを採用
                return Math.Max(total, Constant.GOE_MIN_VALUE);
            }
            else
            {
                // GOE減点なし
                return 0;
            }
        }

        // ジャンプ専用の転倒判定
        private void CheckTesJump(JudgeDetail.Tes tes, SuccessJump successJump)
        {
            if (!successJump())
            {
                // 転倒したことにする
                tes.fall = true;

                // 失敗時はこれまで全審判のGOEを最低点に上書き
                for (var i = 0; i < Constant.REFEREE_COUNT; i++)
                {
                    tes.refereeGoe[i] = Constant.GOE_MIN_VALUE;
                }
            }
        }

        // 技術点の判定結果の記録
        private void RecordTes(JudgeDetail.Tes tes, ProgramComponent component, List<GoeMinus>[][] goeMinus)
        {
            RecordTesByGoeMinus(tes, goeMinus);
            RecordTesByProgramComponentData(tes, component);
        }

        // 減点項目で決まる技術点の判定結果の記録
        private void RecordTesByGoeMinus(JudgeDetail.Tes tes, List<GoeMinus>[][] goeMinus)
        {
            // 審判たちの判定結果を考慮して技術点に影響する減点項目を一つにまとめる
            // TODO:減点項目の採用方法を一人目決め打ちではなくより適切にする
            var geoMinusTesTargets = goeMinus[0];
            foreach (var minus in geoMinusTesTargets)
            {
                if (minus != null && minus.Count() > 0)
                {
                    // 情報記号の構築
                    // 空文字はデータとして扱わない
                    var infoMarks = new JudgeDetail.Tes.InfoMarks
                    {
                        marks = new(minus.Where(x => !Equals(x.mark, "")).Select(x => x.mark))
                    };
                    tes.infoMarksList.Add(infoMarks);
                    // 基礎点倍率の採用
                    tes.baseValueFactors.Add(minus.Min(x => x.baseValueFactor));
                    // ダウングレード判定の採用
                    tes.isDowngrades.Add(minus.Any(x => x.isDowngrade));
                }
                else
                {
                    tes.infoMarksList.Add(new());
                    tes.baseValueFactors.Add(1.0f);
                    tes.isDowngrades.Add(false);
                }
            }
        }

        // プログラム構成データで決まる技術点の判定結果の記録
        private void RecordTesByProgramComponentData(JudgeDetail.Tes tes, ProgramComponent component)
        {
            // 実行済み要素。コンビネーションの場合は+でつなぎ合わせる。各要素の末尾に情報記号を付ける
            if (tes.infoMarksList.Count() == 0)
            {
                throw new Exception("Not recorded tes.infoMarks");
            }
            tes.executedElement = component.elementIds
                .Select((x, i) => (elementId: x, mark: tes.infoMarksList[i].marks))
                .Aggregate("", (a, c) =>
                {
                    var mark = c.mark.Aggregate("", (a, c) => a + c);
                    var id = c.elementId + mark;
                    return Equals(a, "") ? id : $"{a}+{id}";
                });
            // 最終ジャンプかどうか
            tes.lastJump = ProgramUtility.IsLastJumpElementPlaceableSetId(_program, _programComponents, component.elementPlaceableSetId);
            // 基礎点。採点結果にはジャンプボーナス適用後を設定する
            // 違反による基礎点減少倍率はGOEにも影響するのでローカルbaseValueに適用する
            // ダウングレードの場合はダウングレード先の基礎点を参照する
            // ref. https://results.isu.org/results/season2223/wtt2023/data0203.pdf
            if (tes.baseValueFactors.Count() == 0)
            {
                throw new Exception("Not recorded tes.baseValueFactors");
            }
            if (tes.isDowngrades.Count() == 0)
            {
                throw new Exception("Not recorded tes.isDowngrades");
            }
            var lastJumpFactor = tes.lastJump ? 1.1f : 1.0f;
            var baseValue = component.elementIds
                .Select((x, i) => (elementId: x, downgrade: tes.isDowngrades[i], baseValueFactor: tes.baseValueFactors[i]))
                .Sum(x =>
                {
                    // 構成要素のデータからオブジェクトを取得
                    var elementObject = _elementObjectAll.Find(elementObject => Equals(elementObject.data.id, x.elementId)) ?? throw new Exception($"Not found '{x.elementId}'");
                    // 構成要素オブジェクトからダウングレード情報を参照して適切な基礎点を算出
                    var downgrade = x.downgrade && elementObject.downgrade;
                    var basevalue = downgrade ? elementObject.downgrade.data.baseValue : elementObject.data.baseValue;
                    return basevalue * x.baseValueFactor;
                });
            tes.baseValue = baseValue * lastJumpFactor;
            // 全審判の出したGOE平均値から算出される係数と基礎点を掛け合わせてGOEスコアを出す
            // このスコアはジャンプボーナス適用前の基礎点で算出するルール
            // ref. https://www.jsports.co.jp/skate/about/game/
            tes.goeScore = baseValue * (tes.RefereeGoeAverage() * Constant.GOE_SCORE_MAGNIFICATION);
        }

        // 演技構成点の判定
        private void CheckPcs(
            CheckComposition checkComposition,
            CheckPresentation checkPresentation,
            CheckSkatingSkills checkSkatingSkills)
        {
            for (var pcsIndex = 0; pcsIndex < Constant.PCS_COUNT; pcsIndex++)
            {
                var pcs = Detail.pcs[pcsIndex];

                // 審判ごとにスコアを判定
                for (var refereeIndex = 0; refereeIndex < Constant.REFEREE_COUNT; refereeIndex++)
                {
                    // 出来栄え(0-1)を得る
                    var checkResult = pcsIndex switch
                    {
                        0 => checkComposition(), // 構成力(Composition)
                        1 => checkPresentation(), // 演技力(Presentation)
                        2 => checkSkatingSkills(), // スケート技術(Skating Skills)
                        _ => throw new ArgumentOutOfRangeException($"pcsIndex = {pcsIndex}"),
                    };
                    if (0.0 > checkResult || checkResult > 1.0f)
                    {
                        throw new ArgumentOutOfRangeException($"checkResult = {checkResult}");
                    }

                    // 出来栄えから最大値10.0fで0.25刻みのスコアに変換する
                    // ref. https://www.jsports.co.jp/skate/about/game/
                    var rawScore = checkResult * 10.0f;
                    var integerPart = (float)Math.Truncate(rawScore);
                    var decimalPart = rawScore % 1;
                    if (decimalPart < 0.125f) decimalPart = 0.0f;
                    else if (decimalPart < 0.375f) decimalPart = 0.25f;
                    else if (decimalPart < 0.625f) decimalPart = 0.50f;
                    else if (decimalPart < 0.875f) decimalPart = 0.75f;
                    else decimalPart = 1.0f;
                    pcs.refereeScore[refereeIndex] = integerPart + decimalPart;
                }

                // 倍率の設定
                pcs.factor = _program.pcsFactor;
            }
        }

        // 減点判定
        private void CheckDeduction()
        {
            // TODO:Deduction 対応
        }

        // 対象プログラム
        private readonly Program _program;
        // 採点するプログラム構成
        private readonly ProgramComponent[] _programComponents;
        // 採点対象の全構成要素
        private readonly List<ElementObject> _elementObjectAll;

        // GOE減点項目
        private readonly List<GoeMinus>[][][] _goeMinus;
    }
}