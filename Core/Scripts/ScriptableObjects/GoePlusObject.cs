using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GoePlus", menuName = "ScriptableObjects/FigureSkateFramework/GoePlus")]
    public class GoePlusObject : ScriptableObject
    {
        public GoePlus data;
    }
}