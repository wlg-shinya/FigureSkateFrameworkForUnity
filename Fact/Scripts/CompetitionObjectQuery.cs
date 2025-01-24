using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wlg.FigureSkate.Core.Data;
using Wlg.FigureSkate.Core.ScriptableObjects;

namespace Wlg.FigureSkate.Fact
{
    // 大会オブジェクトを得るための問い合わせ
    public static class CompetitionObjectQuery
    {
        // 基準日の年度の全オブジェクトを読み込む
        public static async Task<List<CompetitionObject>> All(YearMonthDay baseday)
        {
            var skateYear = YearMonthDayUtility.GetSkateYearString(baseday);
            return await LoaderUtility.LoadAssetsAsync<CompetitionObject>(@$"Packages/com.welovegamesinc.figureskate-framework/Fact/Objects/{skateYear}/Competition");
        }

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
        public static List<CompetitionObject> ByClassAndSex(List<CompetitionObject> src, List<EventObject> eventObjects, string classId, string sexId)
        {
            return src
                .Where(competitionObject => competitionObject.data.eventIds
                    .Select(x => EventObjectQuery.ById(eventObjects, x))
                    .Any(eventObject => Equals(eventObject.data.classId, classId) && Equals(eventObject.data.sexId, sexId)))
                .ToList();
        }
    }
}
