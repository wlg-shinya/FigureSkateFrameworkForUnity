using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Fact.Data;

namespace Wlg.FigureSkate.Fact
{
    // プログラム構成オブジェクトを得るための問い合わせ
    public static class ProgramObjectQuery
    {
        // 指定した選手にあったオブジェクトをすべて得る
        public static List<ProgramObject> ByPlayer(CompetitionObject competitionObject, List<EventObject> eventObjects, Player player)
        {
            var list = competitionObject.data.eventIds
                .Select(x => EventObjectQuery.ById(eventObjects, x))
                .Where(eventObject => Equals(eventObject.data.classId, player.classId) && Equals(eventObject.data.sexId, player.sexId))
                .SelectMany(eventObject => eventObject.programObjects)
                .ToList();
            if (list.Count() <= 0)
            {
                throw new Exception($"Not found programObject in {competitionObject.name}");
            }
            return list;
        }

        // 指定した選手にあったオブジェクトを条件セットアップ済みにしてすべて得る
        public static List<ProgramObject> ByPlayerWithSetupConditions(CompetitionObject competitionObject, List<EventObject> eventObjects, Player player)
        {
            return ByPlayer(competitionObject, eventObjects, player).Select(programObject => SetupConditions(programObject)).ToList();
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
