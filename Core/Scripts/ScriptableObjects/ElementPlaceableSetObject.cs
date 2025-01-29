using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementPlaceableSet", menuName = "ScriptableObjects/ElementPlaceableSet")]
    public class ElementPlaceableSetObject : ScriptableObject
    {
        public ElementPlaceableSet data;
    }
}