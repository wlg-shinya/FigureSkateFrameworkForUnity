using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Sex", menuName = "ScriptableObjects/Sex")]
    public class SexObject : ScriptableObject
    {
        public Sex data;
    }
}