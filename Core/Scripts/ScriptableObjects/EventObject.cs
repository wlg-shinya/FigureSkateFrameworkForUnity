using System.Collections.Generic;
using Wlg.FigureSkate.Core.Data;
using UnityEngine;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/Event")]
    public class EventObject : ScriptableObject
    {
        public Data.Event data;
        // 対象クラス
        public ClassObject targetClassObject;
        // 対象性別
        public SexObject targetSexObject;
        // このイベントに含まれる全プログラム
        public List<ProgramObject> programObjects;
    }
}