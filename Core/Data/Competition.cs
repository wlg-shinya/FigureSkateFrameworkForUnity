using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 大会
    [Serializable]
    public class Competition
    {
        // ID
        public string id;
        // 名称
        public string name;
        // 開催開始日
        public YearMonthDay startDay;
        // 開催終了日
        public YearMonthDay endDay;
    }
}