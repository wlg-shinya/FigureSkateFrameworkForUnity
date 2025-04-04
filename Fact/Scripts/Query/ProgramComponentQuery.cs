using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core;

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

        // 指定プログラム構成が指定プログラム構成規則に則っているか
        public static bool Verify(
            ProgramComponent[] components,
            ProgramComponentRegulation programComponentRegulation,
            List<ElementPlaceableSetObject> elementPlaceableSetObjectAll
            )
        {
            if (components == null)
            {
                // データが存在していない
                return false;
            }
            else if (components.Length != programComponentRegulation.elementPlaceableSetIds.Length)
            {
                // 構成数が不一致
                return false;
            }
            else
            {
                {
                    var idsInRegulation = programComponentRegulation.elementPlaceableSetIds.OrderBy(x => x);
                    var idsInComponents = components.Select(x => x.elementPlaceableSetId).OrderBy(x => x);
                    if (!idsInRegulation.SequenceEqual(idsInComponents))
                    {
                        // 現在の構成と規則上の構成の項目内容が不一致
                        return false;
                    }
                }
                {
                    var idsInRegulation = programComponentRegulation.elementPlaceableSetIds
                        .Select(id => elementPlaceableSetObjectAll.Find(x => x.data.id.Equals(id)))
                        .Where(x => x != null)
                        .SelectMany(obj => obj.data.elementPlaceableIds)
                        .Distinct();
                    var idsInComponents = components
                        .SelectMany(x => x.elementIds)
                        .Distinct()
                        .Where(x => !string.IsNullOrEmpty(x));
                    if (!idsInComponents.All(x => idsInRegulation.Any(y => y.Equals(x))))
                    {
                        // 現在構成されている要素が規則上許可されていない要素
                        return false;
                    }
                }
            }

            // すべての検証を通過
            return true;
        }
    }
}
