using System;

namespace Wlg.FigureSkate.Core
{
    // 年月日。値の扱いはDateTimeと同じ https://learn.microsoft.com/ja-jp/dotnet/api/system.DateTime?view=net-8.0
    [Serializable]
    public class YearMonthDay : IComparable
    {
        // 西暦年(0-*)。2024 = 2024年   
        public int year;
        // 月(1-12)。1 = 1月
        public int month;
        // 日(1-31)。1 = 1日
        public int day;

        public YearMonthDay()
        {
            Clear();
        }
        public YearMonthDay(YearMonthDay yearMonthDay)
        {
            year = yearMonthDay.year;
            month = yearMonthDay.month;
            day = yearMonthDay.day;
        }
        public YearMonthDay(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
        }
        // MEMO:年月日は"/"区切りでローカライズ影響を受けない
        public YearMonthDay(string yearMonthDayString)
        {
            var s = yearMonthDayString.Split('/');
            if (s.Length != 3)
            {
                throw new ArgumentException("Not found separator '/' (ex. 2025/1/23)");
            }
            year = int.Parse(s[0]);
            month = int.Parse(s[1]);
            day = int.Parse(s[2]);
        }

        #region // 演算子オーバーロード
        public static bool operator ==(YearMonthDay a, YearMonthDay b) => a.Equals(b);
        public static bool operator !=(YearMonthDay a, YearMonthDay b) => !(a == b);
        public static bool operator <(YearMonthDay a, YearMonthDay b)
        {
            if (a.year != b.year) return a.year < b.year;
            else if (a.month != b.month) return a.month < b.month;
            else if (a.day != b.day) return a.day < b.day;
            else return false; // 全部一致しているので false
        }
        public static bool operator >(YearMonthDay a, YearMonthDay b) => b < a;
        public static bool operator <=(YearMonthDay a, YearMonthDay b)
        {
            if (a == b) return true;
            else if (a < b) return true;
            else return false;
        }
        public static bool operator >=(YearMonthDay a, YearMonthDay b) => b <= a;
        #endregion

        #region // Objectオーバーライド
        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var a = (YearMonthDay)obj;
            return year == a.year && month == a.month && day == a.day;
        }
        public override int GetHashCode() => (year, month, day).GetHashCode();
        public override string ToString()
        {
            // TODO:ローカライズ対応
            return $"{year}/{month}/{day}";
        }
        #endregion

        #region // IComparable実装
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var other = obj as YearMonthDay;
            if (this == other) return 0;
            else if (this > other) return 1;
            return -1;
        }
        #endregion

        // 年月日情報のコピー
        public void Copied(YearMonthDay src)
        {
            year = src.year;
            month = src.month;
            day = src.day;
        }

        // 指定日数分追加した日付にする
        public void AddDay(int value)
        {
            var currentDatetime = new DateTime(year, month, day);
            var newDatetime = currentDatetime.AddDays(value);
            year = newDatetime.Year;
            month = newDatetime.Month;
            day = newDatetime.Day;
        }

        // メンバ変数初期化
        public void Clear()
        {
            year = 0;
            month = 1;
            day = 1;
        }
    }
}