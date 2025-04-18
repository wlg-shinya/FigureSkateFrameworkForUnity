using System;
using System.Collections.Generic;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成要素の規則
    [Serializable]
    public class ProgramComponentRegulation
    {
        public string id;
        public string[] elementPlaceableSetIds;

        public List<ProgramComponentCondition> Conditions { get; private set; } = new(); // このプロパティはプログラム上でセットアップする
    }
}