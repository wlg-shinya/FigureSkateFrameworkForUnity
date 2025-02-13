using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/FigureSkateFramework/Class")]
    public class ClassObject : ScriptableObject
    {
        public Class data;
    }
}