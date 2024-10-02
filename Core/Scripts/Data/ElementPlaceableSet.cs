using System;
using System.Collections.Generic;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム構成の1項目を表現したデータ
    [Serializable]
    public class ElementPlaceableSet
    {
        // ID
        public string id;
        // 名称
        public string name;
        // ジャンプに関する規則か
        public bool jump;
        // プログラム構成に設定可能な構成要素群。ジャンプの単独かコンビネーションかを表現
        public ElementPlaceable[] elementPlaceables;
        // 項目内が規則に準じているかの判定群
        public List<ElementPlaceableSetCondition> Conditions { get; private set; } = new(); // このプロパティはプログラム上でセットアップする
    }
}