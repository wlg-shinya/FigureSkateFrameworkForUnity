using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 減点項目
    [Serializable]
    public class Deduction
    {
        // 名称
        public string name;
        // 説明
        public string description;
        // 減点される値
        public float value;
    }
}