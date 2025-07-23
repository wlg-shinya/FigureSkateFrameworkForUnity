using System;
using Wlg.FigureSkate.Core;

namespace Wlg.FigureSkate.Fact
{
    public static class YearMonthDayUtility
    {
        // 満年齢を得る
        public static int GetAge(YearMonthDay baseday, YearMonthDay birthday)
        {
            // 今年も含めた満年齢を算出
            var age = baseday.year - birthday.year;
            // 月日の比較にて今年まだ誕生日を迎えていない場合は-1
            var basedayClone = new YearMonthDay(baseday);
            basedayClone.year -= age;
            if (birthday > basedayClone)
            {
                age--;
            }
            return age;
        }

        // 基準日のスケート年度文字列(ex. 2023-24)を得る
        public static string GetSkateYearString(YearMonthDay baseday)
        {
            // 1月〜6月は前の年のシーズンに属する
            int seasonStartYear = baseday.month < 7 ? baseday.year - 1 : baseday.year;
            return $"{seasonStartYear}-{(seasonStartYear + 1) % 100:D2}";
        }

        // 指定シーズンの最終日を得る
        public static YearMonthDay GetSeasonLastDay(string season)
        {
            // "2024-25" -> "2024" と "25" に分割
            var parts = season.Split('-');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int startYear))
            {
                // 不正なフォーマット
                throw new ArgumentException("Invalid season format. Expected 'YYYY-YY'.", nameof(season));
            }

            // 終了年は開始年の翌年
            int endYear = startYear + 1;

            // シーズンの最終日は常に6月30日
            return new YearMonthDay(endYear, 6, 30);
        }
    }
}
