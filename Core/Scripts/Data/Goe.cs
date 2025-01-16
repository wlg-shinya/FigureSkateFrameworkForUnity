using System;

namespace Wlg.FigureSkate.Core.Data
{
    // Grade Of Execution
    [Serializable]
    public class Goe
    {
        // 一意の名称
        public string id;
        // 加点項目
        public GoePlus[] plus = new GoePlus[Constant.GOE_PLUS_LENGTH];
        // 減点項目
        public GoeMinus[] minus;
    }
}