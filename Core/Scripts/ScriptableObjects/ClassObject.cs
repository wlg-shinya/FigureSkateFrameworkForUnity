using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class")]
    public class ClassObject : ScriptableObject
    {
        public Class data;
    }
}