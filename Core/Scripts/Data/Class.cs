using System;
using UnityEngine.Localization;

namespace Wlg.FigureSkate.Core
{
    // クラス（シニア、ジュニアなど）
    [Serializable]
    public class Class
    {
        // 一意の記号
        public string id;
        // 名称
        public LocalizedString name;
        // 最低年齢
        public int minAge;
        // 最高年齢
        public int maxAge;
    }
}