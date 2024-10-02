using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 構成要素（ダブルアクセル、シットスピンなど）
    [Serializable]
    public class Element
    {
        // 一意の記号
        public string id;
        // 正式名称
        public string name;
        // 基礎点
        public float baseValue;
    }
}