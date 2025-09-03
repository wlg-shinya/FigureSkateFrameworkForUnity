using System;
using System.Collections.Generic;

namespace Wlg.FigureSkate.Fact
{
    [Serializable]
    public class ValidProgramComponents
    {
        public Dictionary<ushort, ValidProgramComponents> Children = new();
        public bool IsEndOfValid { get; set; }
        public float TotalBaseValue { get; set; }
    }
}