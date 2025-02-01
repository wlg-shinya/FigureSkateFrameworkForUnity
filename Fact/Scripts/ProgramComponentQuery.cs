using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成データを得るための問い合わせ
    public static class ProgramComponentQuery
    {
        // 指定プログラムの構成を組めるように各種要素をnewした ProgramComponent[] を作成する
        public static ProgramComponent[] Create(ProgramComponentRegulation programComponentRegulation, List<ElementPlaceableSetObject> elementPlaceableSetObjectAll)
        {
            var componentLength = programComponentRegulation.elementPlaceableSetIds.Length;
            var programComponents = new ProgramComponent[componentLength];
            for (var i = 0; i < componentLength; i++)
            {
                var elementPlaceableSet = ElementPlaceableSetObjectQuery.ById(elementPlaceableSetObjectAll, programComponentRegulation.elementPlaceableSetIds[i]).data;
                programComponents[i] = new()
                {
                    elementPlaceableSetId = elementPlaceableSet.id,
                    elementIds = new string[elementPlaceableSet.elementPlaceableIds.Length]
                };
            }

            return programComponents;
        }

        // 指定プログラムの構成として使えるか検証
        public static bool Verify(ProgramComponent[] components, ProgramComponentRegulation programComponentRegulation)
        {
            if (components == null || components.Length != programComponentRegulation.elementPlaceableSetIds.Length)
            {
                // 構成数が不一致
                return false;
            }
            else
            {
                var idsInRegulation = programComponentRegulation.elementPlaceableSetIds;
                var idsInComponents = components.Select(x => x.elementPlaceableSetId);
                var idsUnique = idsInRegulation.Concat(idsInComponents).Distinct();
                if (idsInRegulation.Count() != idsUnique.Count())
                {
                    // 構成要素IDが不一致
                    return false;
                }
                // MEMO:同じ elementPlaceableSetId 同士の構成要素数が一致しているかまでチェックすると検証はより厳密になる。そこまで必要じゃないので未実装
            }

            // すべての検証を通過
            return true;
        }
    }
}
