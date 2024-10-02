using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Program", menuName = "ScriptableObjects/Program")]
    public class ProgramObject : ScriptableObject
    {
        // プログラム構成一つに対する構成要素条件とそれを適用するプログラム構成IDをセットにしたデータ
        [Serializable]
        public class ElementPlaceableSetConditionObjectData
        {
            public string id;
            public ElementPlaceableSetConditionObject obj;
        }

        public Data.Program data;
        // プログラム構成一つに対する構成要素条件群
        public List<ElementPlaceableSetConditionObjectData> elementPlaceableSetConditionObjectDataList;
        // プログラム構成全体に対する構成要素条件群
        public List<ProgramComponentConditionObject> ProgramComponentConditionObjects;
    }
}