using System;
using UnityEngine.Localization;

namespace Wlg.FigureSkate.Core
{
    // 大会
    [Serializable]
    public class Competition
    {
        // 一意の記号
        public string id;
        // 開催地
        public string regionId;
        // 開催国コード
        public string countryId;
        // 名称
        public LocalizedString name;
        // 国際大会か
        public bool isInternational;
        // 開催開始日
        public YearMonthDay startDay;
        // 開催終了日
        public YearMonthDay endDay;
        // 開催イベントID群
        public string[] eventIds;
    }
}