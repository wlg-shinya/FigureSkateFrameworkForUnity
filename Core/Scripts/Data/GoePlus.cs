using System;

namespace Wlg.FigureSkate.Core.Data
{
    // Grade Of Execution の加点項目のいち要素
    [Serializable]
    public class GoePlus
    {
        // 一意の記号
        public string id;
        // カテゴリ
        public string category;
        // チェックの説明
        public string description;
        // 重要項目か（この要素はGOE加点に制約を発生させるか）
        public bool important;
    }
}