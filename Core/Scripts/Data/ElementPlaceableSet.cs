using System;
using System.Collections.Generic;

namespace Wlg.FigureSkate.Core
{
    // プログラム構成の1項目を表現したデータ
    [Serializable]
    public class ElementPlaceableSet
    {
        // 一意の記号
        public string id;
        // 名称
        public string name;
        // ジャンプに関する規則か
        public bool jump;
        // プログラム構成に設定可能な構成要素ID群。コンビネーションジャンプの場合は配列要素数が増加する
        public string[] elementPlaceableIds;

        // 項目内が規則に準じているかの判定群
        public List<ElementPlaceableSetCondition> Conditions { get; private set; } = new(); // このプロパティはプログラム上でセットアップする
    }
}