using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Fact.Utility
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
            // スケートは7/1が年度初め
            if (baseday.month <= 6)
            {
                return $"{baseday.year - 1}-{baseday.year % 100}";
            }
            else
            {
                return $"{baseday.year}-{(baseday.year % 100) + 1}";
            }
        }
    }
}
