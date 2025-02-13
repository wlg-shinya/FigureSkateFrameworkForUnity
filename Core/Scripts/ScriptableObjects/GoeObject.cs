using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Goe", menuName = "ScriptableObjects/FigureSkateFramework/Goe")]
    public class GoeObject : ScriptableObject
    {
        public Goe data;
    }
}