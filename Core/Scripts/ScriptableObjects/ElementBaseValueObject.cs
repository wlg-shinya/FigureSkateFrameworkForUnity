using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementBaseValue", menuName = "ScriptableObjects/ElementBaseValue")]
    public class ElementBaseValueObject : ScriptableObject
    {
        public ElementBaseValue data;
    }
}
