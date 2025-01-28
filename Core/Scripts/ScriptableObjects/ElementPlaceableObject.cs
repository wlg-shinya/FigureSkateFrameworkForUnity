using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementPlaceable", menuName = "ScriptableObjects/ElementPlaceable")]
    public class ElementPlaceableObject : ScriptableObject
    {
        public ElementPlaceable data;
    }
}