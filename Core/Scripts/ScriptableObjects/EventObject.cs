using System.Collections.Generic;
using UnityEngine;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/Event")]
    public class EventObject : ScriptableObject
    {
        public Data.Event data;
        // このイベントに含まれる全プログラム
        public List<ProgramObject> programObjects;
    }
}