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
            if (src == null) throw new Exception($"src is null");
            return src.Find(x => Equals(x.data.id, id)) ?? throw new Exception($"Not found '{id}'");
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
        public static List<ProgramObject> ByPlayerWithSetupConditions(CompetitionObject competitionObject, List<EventObject> eventObjects, List<ProgramObject> programObjects, Player player)
        {
            return ByPlayer(competitionObject, eventObjects, programObjects, player).Select(programObject => SetupConditions(programObject)).ToList();
        }

        // 指定したプログラムの構成要素設定条件のセットアップ
        public static ProgramObject SetupConditions(ProgramObject programObject)
        {
            // 構成ひとつに対する条件設定をプログラムにつなぎ合わせる
            foreach (var data in programObject.elementPlaceableSetConditionObjectDataList)
            {
                var elementPlaceableSet = programObject.data.regulation.elementPlaceableSets.ToList().Find(x => x.id == data.id) ?? throw new Exception($"Not found id '{data.id}' in elementPlaceableSets");
                elementPlaceableSet.Conditions.Add(data.obj.Data());
            }
            // 構成全体に対する条件設定をプログラムにつなぎ合わせる
            foreach (var obj in programObject.ProgramComponentConditionObjects)
            {
                programObject.data.regulation.Conditions.Add(obj.Data());
            }
            return programObject;
        }
    }
}
