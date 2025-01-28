using System;

namespace Wlg.FigureSkate.Core.Data
{
    // Grade Of Execution
    [Serializable]
    public class Goe
    {
        // 一意の記号
        public string id;
        // 加点項目ID群
        public string[] plusIds;
        // 減点項目ID群
        public string[] minusIds;
    }
}