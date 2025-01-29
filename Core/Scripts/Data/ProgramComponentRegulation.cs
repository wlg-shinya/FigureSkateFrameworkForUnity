using System;
using System.Collections.Generic;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム構成要素の規則
    [Serializable]
    public class ProgramComponentRegulation
    {
        public string id;
        public string[] elementPlaceableSetIds;

        public ElementPlaceableSet[] elementPlaceableSets;
        public List<ProgramComponentCondition> Conditions { get; private set; } = new(); // このプロパティはプログラム上でセットアップする
    }
}