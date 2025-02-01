using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;

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
            if (Constant.GOE_MIN_VALUE > goe || goe > Constant.GOE_MAX_VALUE)
            {
                throw new ArgumentOutOfRangeException($"goe = {goe}");
            }
            return components.Aggregate(0.0f, (a1, c1) =>
            {
                var factor = IsLastJumpElementPlaceableSetId(program, components, elementPlaceableSetAll, c1.elementPlaceableSetId) ? 1.1f : 1.0f;
                return c1.elementIds.Aggregate(0.0f, (a2, c2) =>
                {
                    var elementObject = Array.Find(elementBaseValueAll, x => x.id.Equals(c2));
                    var baseValue = elementObject.baseValue;
                    switch (goe)
                    {
                        case -5: baseValue += elementObject.baseValueM5; break;
                        case -4: baseValue += elementObject.baseValueM4; break;
                        case -3: baseValue += elementObject.baseValueM3; break;
                        case -2: baseValue += elementObject.baseValueM2; break;
                        case -1: baseValue += elementObject.baseValueM1; break;
                        case 1: baseValue += elementObject.baseValueP1; break;
                        case 2: baseValue += elementObject.baseValueP2; break;
                        case 3: baseValue += elementObject.baseValueP3; break;
                        case 4: baseValue += elementObject.baseValueP4; break;
                        case 5: baseValue += elementObject.baseValueP5; break;
                    }
                    return (elementObject != null ? baseValue * factor : 0.0f) + a2;
                }) + a1;
            });
        }

        // 指定データはジャンプボーナス対象か
        public static bool IsLastJumpElementPlaceableSetId(Program program, ProgramComponent[] components, ElementPlaceableSet[] elementPlaceableSetAll, string elementPlaceableSetId)
        {
            return LastJumpElementPlaceableSetIds(program, components, elementPlaceableSetAll).Any(x => x.Equals(elementPlaceableSetId));
        }

        // ジャンプボーナス対象の構成一覧
        public static IEnumerable<string> LastJumpElementPlaceableSetIds(Program program, ProgramComponent[] components, ElementPlaceableSet[] elementPlaceableSetAll)
        {
            var jumps = components.Where(x => GetElementPlaceableSetById(elementPlaceableSetAll, x.elementPlaceableSetId).jump);
            return jumps
                // 末尾から最終ジャンプ数だけデータを残す
                .Select((x, i) => jumps.Count() - i <= program.lastJumpSpecialFactorCount ? x : null)
                .Where(x => x != null)
                // 構成要素項目IDのみを一覧として返す
                .Select(x => x.elementPlaceableSetId);
        }
    }
}