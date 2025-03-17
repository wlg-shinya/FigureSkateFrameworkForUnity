using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/FigureSkateFramework/Event")]
    public class EventObject : ScriptableObject
    {
        public Event data;
    }
}