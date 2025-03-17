using System;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成ひとつ分のデータ。これを配列で扱うことでプログラム構成全体のデータとします
    [Serializable]
    public class ProgramComponent
    {
        public string elementPlaceableSetId;
        public string[] elementIds;
    }
}