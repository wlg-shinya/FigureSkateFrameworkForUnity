using System;
using System.Collections.Generic;
using System.Linq;
using Wlg.FigureSkate.Core.ScriptableObjects;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Fact
{
    // クラスオブジェクトを得るための問い合わせ
    public static class ClassObjectQuery
    {
        // 指定IDのオブジェクトを得る
        public static ClassObject ById(List<ClassObject> src, string id) => src.Find(x => x.data.id == id) ?? throw new Exception($"Not found '{id}'");

        // 適切なものをリストで作成。複数当てはまる場合はすべてリストに含める
        public static List<ClassObject> ByBirthday(List<ClassObject> src, YearMonthDay baseday, YearMonthDay birthday)
        {
            // 基準日より誕生日のほうが未来だったらエラー
            if (baseday < birthday)
            {
                throw new Exception("birthday is over baseday");
            }

            // 年齢の算出
            var age = YearMonthDayUtility.GetAge(baseday, birthday);

            // 条件に当てはまるクラスオブジェクトを列挙
            var filtered = src.Where(x => x.data.minAge <= age && age <= x.data.maxAge);
            if (filtered.Count() == 0)
            {
                throw new Exception("Not found ClassObject");
            }

            return filtered.ToList();
        }
    }
}
