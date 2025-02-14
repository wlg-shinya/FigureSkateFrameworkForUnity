using System;
using System.Collections.Generic;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Fact.Data
{
    // 選手
    [Serializable]
    public class Player
    {
        // 名前
        public string name;
        // スコア上の名前
        public string nameOnScore;
        // 生年月日
        public YearMonthDay birthday;
        // 性別
        public string sexId;
        // クラス
        public string classId;
        // 国籍
        public string nation;
        // 実行可能な構成要素
        public List<string> elementIdList;
        // プログラム構成
        [Serializable]
        public class ProgramComponents
        {
            public ProgramComponent[] components;
        }
        public ProgramComponents[] programComponentsList;
    }
}