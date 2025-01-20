using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 大会
    [Serializable]
    public class Competition
    {
        // 一意の記号
        public string id;
        // 名称
        public string name;
        // 開催開始日
        public YearMonthDay startDay;
        // 開催終了日
        public YearMonthDay endDay;
    }
}