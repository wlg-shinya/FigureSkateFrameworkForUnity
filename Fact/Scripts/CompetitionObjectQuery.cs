using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // 大会オブジェクトを得るための問い合わせ
    public static class CompetitionObjectQuery
    {
        // 指定IDのオブジェクトを得る
        public static CompetitionObject ById(List<CompetitionObject> src, string id) => src.Find(x => x.data.id == id) ?? throw new Exception($"Not found '{id}'");

        // 指定日以降の開催日の大会すべてを得る
        public static List<CompetitionObject> StartDayAfterBaseday(List<CompetitionObject> src, YearMonthDay baseday)
        {
            return src
                .Where(competitionObject => competitionObject.data.startDay >= baseday)
                .ToList();
        }

        // 指定クラス・性別のイベントがある大会すべてを得る
        public static List<CompetitionObject> ByClassAndSex(List<CompetitionObject> src, string classId, string sexId)
        {
            return src
                .Where(competitionObject => competitionObject.eventObjects
                    .Any(eventObject => Equals(eventObject.targetClassObject.data.id, classId) && Equals(eventObject.targetSexObject.data.id, sexId)))
                .ToList();
        }
    }
}
