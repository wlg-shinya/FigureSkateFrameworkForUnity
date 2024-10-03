using System.Linq;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成データを得るための問い合わせ
    public static class ProgramComponentQuery
    {
        // 指定プログラムの構成を組めるように各種要素をnewした ProgramComponent[] を作成する
        public static ProgramComponent[] Create(Program program)
        {
            var componentLength = program.regulation.elementPlaceableSets.Length;
            var programComponents = new ProgramComponent[componentLength];
            for (var i = 0; i < componentLength; i++)
            {
                var elementPlaceableSet = program.regulation.elementPlaceableSets[i];
                programComponents[i] = new()
                {
                    elementPlaceableSetId = elementPlaceableSet.id,
                    elementIds = new string[elementPlaceableSet.elementPlaceables.Length]
                };
            }

            return programComponents;
        }

        // 指定プログラムの構成として使えるか検証
        public static bool Verify(ProgramComponent[] components, Program program)
        {
            if (components == null || components.Length != program.regulation.elementPlaceableSets.Length)
            {
                // 構成数が不一致
                return false;
            }
            else
            {
                var idsInRegulation = program.regulation.elementPlaceableSets.Select(x => x.id);
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
