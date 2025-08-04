using System;
using UnityEngine.Localization;

namespace Wlg.FigureSkate.Core
{
    // 性別
    [Serializable]
    public class Sex
    {
        // 一意の記号
        public string id;
        // 名称
        public LocalizedString name;
    }
}