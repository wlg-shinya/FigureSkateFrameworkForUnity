using UnityEngine;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/FigureSkateFramework/Event")]
    public class EventObject : ScriptableObject
    {
        public Data.Event data;
    }
}