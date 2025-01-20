using System;

namespace Wlg.FigureSkate.Core.Data
{
    // 種目（男子シングル、ペアなど）
    [Serializable]
    public class Event
    {
        // 一意の記号
        public string id;
        // 名称
        public string name;
        // 対象クラスID
        public string classId;
        // 対象性別ID
        public string sexId;
    }
}