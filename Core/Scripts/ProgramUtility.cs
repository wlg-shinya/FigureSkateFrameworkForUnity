using System;
using System.Collections.Generic;
using System.Linq;

namespace Wlg.FigureSkate.Core
{
    // プログラムに関するユーティリティクラス
    public static class ProgramUtility
    {
        // プログラム構成要素の規則データを指定IDから得る
        public static ProgramComponentRegulation GetProgramComponentRegulationById(ProgramComponentRegulation[] programComponentRegulationAll, string id)
        {
            return Array.Find(programComponentRegulationAll, x => x.id.Equals(id));
        }

        // 構成要素項目データを指定IDから得る
        public static ElementPlaceableSet GetElementPlaceableSetById(ElementPlaceableSet[] elementPlaceableSetAll, string id)
        {
            return Array.Find(elementPlaceableSetAll, x => x.id.Equals(id));
        }

        // 登録した構成要素の合計基礎点（ジャンプボーナス込み、GOE考慮）の見積もり算出
        // 正式な値は Judge をする必要がある。これで得られるものはあくまでも机上の値
        public static float EstimateTotalBaseValue(Program program, ProgramComponent[] components, ElementPlaceableSet[] elementPlaceableSetAll, ElementBaseValue[] elementBaseValueAll, int goe)
        {
            if (CoreConstant.GOE_MIN_VALUE > goe || goe > CoreConstant.GOE_MAX_VALUE)
            {
                throw new ArgumentOutOfRangeException($"goe = {goe}");
            }
            var result = components.Aggregate(0.0f, (a1, c1) =>
            {
                var factor = IsLastJumpProgramComponent(program, components, elementPlaceableSetAll, c1) ? 1.1f : 1.0f;
                return c1.elementIds.Aggregate(0.0f, (a2, c2) =>
                {
                    var elementBaseValue = Array.Find(elementBaseValueAll, x => x.id.Equals(c2));
                    if (elementBaseValue == null)
                    {
                        return a2;
                    }
                    else
                    {
                        var baseValue = elementBaseValue.baseValue;
                        switch (goe)
                        {
                            case -5: baseValue += elementBaseValue.baseValueM5; break;
                            case -4: baseValue += elementBaseValue.baseValueM4; break;
                            case -3: baseValue += elementBaseValue.baseValueM3; break;
                            case -2: baseValue += elementBaseValue.baseValueM2; break;
                            case -1: baseValue += elementBaseValue.baseValueM1; break;
                            case 1: baseValue += elementBaseValue.baseValueP1; break;
                            case 2: baseValue += elementBaseValue.baseValueP2; break;
                            case 3: baseValue += elementBaseValue.baseValueP3; break;
                            case 4: baseValue += elementBaseValue.baseValueP4; break;
                            case 5: baseValue += elementBaseValue.baseValueP5; break;
                        }
                        return (baseValue * factor) + a2;
                    }
                }) + a1;
            });
            return JudgeDetail.Round(result);
        }

        // 指定データはジャンプボーナス対象か
        public static bool IsLastJumpProgramComponent(Program program, ProgramComponent[] components, ElementPlaceableSet[] elementPlaceableSetAll, ProgramComponent targetProgramComponent)
        {
            return LastJumpProgramComponents(program, components, elementPlaceableSetAll).Any(x => ReferenceEquals(x, targetProgramComponent));
        }

        // ジャンプボーナス対象の構成一覧
        public static IEnumerable<ProgramComponent> LastJumpProgramComponents(Program program, ProgramComponent[] components, ElementPlaceableSet[] elementPlaceableSetAll)
        {
            var jumps = components.Where(x => GetElementPlaceableSetById(elementPlaceableSetAll, x.elementPlaceableSetId).jump);
            return jumps
                // 末尾から最終ジャンプ数だけデータを残す
                .Select((x, i) => jumps.Count() - i <= program.lastJumpSpecialFactorCount ? x : null)
                .Where(x => x != null);
        }
    }
}