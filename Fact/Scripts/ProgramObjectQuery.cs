using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact.Data;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成オブジェクトを得るための問い合わせ
    public static class ProgramObjectQuery
    {
        // 基準日の年度の全オブジェクトを読み込む
        public static async Task<List<ProgramObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<ProgramObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/Program");
        }

        // 指定オブジェクト群から指定IDのオブジェクト単体を得る
        public static ProgramObject ById(List<ProgramObject> src, string id)
        {
            if (src == null) throw new ArgumentException($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
        }

        // 指定オブジェクト群から複数の指定IDのオブジェクトをすべて得る
        public static List<ProgramObject> ByIds(List<ProgramObject> src, string[] ids)
        {
            if (src == null) throw new ArgumentException($"src is null");
            if (ids.Length == 0) throw new ArgumentException($"ids is empty");
            var result = src.Where(obj => ids.Any(id => obj.data.id.Equals(id)));
            if (result.Count() == 0) throw new Exception($"Not found ids([0]='{ids[0]}')");
            return result.ToList();
        }

        // 指定した選手にあったオブジェクトをすべて得る
        public static List<ProgramObject> ByPlayer(CompetitionObject competitionObject, List<EventObject> eventObjects, List<ProgramObject> programObjects, Player player)
        {
            var list = competitionObject.data.eventIds
                .Select(x => EventObjectQuery.ById(eventObjects, x))
                .Where(eventObject => Equals(eventObject.data.classId, player.classId) && Equals(eventObject.data.sexId, player.sexId))
                .SelectMany(eventObject => eventObject.data.programIds.Select(x => ById(programObjects, x)))
                .ToList();
            if (list.Count() <= 0)
            {
                throw new Exception($"Not found programObject in {competitionObject.name}");
            }
            return list;
        }

        // 指定した選手にあったオブジェクトを条件セットアップ済みにしてすべて得る
        public static List<ProgramObject> ByPlayerWithSetupConditions(
            CompetitionObject competitionObject,
            List<EventObject> eventObjects,
            List<ProgramObject> programObjects,
            List<ProgramComponentRegulationObject> programComponentRegulationObjects,
            List<ElementPlaceableSetObject> elementPlaceableSetObjects,
            Player player
            )
        {
            return ByPlayer(competitionObject, eventObjects, programObjects, player)
                .Select(programObject => SetupConditions(programObject, programComponentRegulationObjects, elementPlaceableSetObjects))
                .ToList();
        }

        // 指定したプログラムの構成要素設定条件のセットアップ
        public static ProgramObject SetupConditions(
            ProgramObject programObject,
            List<ProgramComponentRegulationObject> programComponentRegulationObjects,
            List<ElementPlaceableSetObject> elementPlaceableSetObjects
            )
        {
            // 構成ひとつに対する条件設定をプログラムにつなぎ合わせる
            foreach (var data in programObject.elementPlaceableSetConditionObjectDataList)
            {
                var elementPlaceableSet = ElementPlaceableSetObjectQuery.ById(elementPlaceableSetObjects, data.id).data;
                elementPlaceableSet.Conditions.Add(data.obj.Data());
            }
            // 構成全体に対する条件設定をプログラムにつなぎ合わせる
            var regulation = ProgramComponentRegulationObjectQuery.ById(programComponentRegulationObjects, programObject.data.programComponentRegulationId).data;
            foreach (var obj in programObject.ProgramComponentConditionObjects)
            {
                regulation.Conditions.Add(obj.Data());
            }
            return programObject;
        }
    }
}
