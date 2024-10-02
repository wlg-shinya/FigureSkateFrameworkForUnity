using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Core
{
    // プログラム関するユーティリティクラス
    public static class ProgramUtility
    {
        // 構成要素項目オブジェクトを指定IDから得る
        public static ElementPlaceableSet ElementPlaceableSetById(Program program, string id) => program.regulation.elementPlaceableSets.ToList().Find(x => x.id == id) ?? throw new Exception($"Not found '{id}'");

        // 登録した構成要素の合計基礎点（ジャンプボーナス込み、GOE考慮）の見積もり算出
        // 正式な値は Judge をする必要がある。これで得られるものはあくまでも机上の値
        public static float EstimateTotalBaseValue(Program program, ProgramComponent[] components, List<ElementObject> elementObjects, int goe)
        {
            if (Constant.GOE_MIN_VALUE > goe || goe > Constant.GOE_MAX_VALUE)
            {
                throw new ArgumentOutOfRangeException($"goe = {goe}");
            }
            var goeFactor = 1.0f + (Constant.GOE_SCORE_MAGNIFICATION * goe);
            return components.Aggregate(0.0f, (a1, c1) =>
            {
                var factor = IsLastJumpElementPlaceableSetId(program, components, c1.elementPlaceableSetId) ? 1.1f : 1.0f;
                return c1.elementIds.Aggregate(0.0f, (a2, c2) =>
                {
                    var elementObject = elementObjects.Find(x => x.data.id.Equals(c2));
                    return (elementObject != null ? elementObject.data.baseValue * goeFactor * factor : 0.0f) + a2;
                }) + a1;
            });
        }

        // 指定データはジャンプボーナス対象か
        public static bool IsLastJumpElementPlaceableSetId(Program program, ProgramComponent[] components, string elementPlaceableSetId)
        {
            return LastJumpElementPlaceableSetIds(program, components).Any(x => x.Equals(elementPlaceableSetId));
        }

        // ジャンプボーナス対象の構成一覧
        public static IEnumerable<string> LastJumpElementPlaceableSetIds(Program program, ProgramComponent[] components)
        {
            var jumps = components.Where(x => ElementPlaceableSetById(program, x.elementPlaceableSetId).jump);
            return jumps
                // 末尾から最終ジャンプ数だけデータを残す
                .Select((x, i) => jumps.Count() - i <= program.lastJumpSpecialFactorCount ? x : null)
                .Where(x => x != null)
                // 構成要素項目IDのみを一覧として返す
                .Select(x => x.elementPlaceableSetId);
        }
    }
}