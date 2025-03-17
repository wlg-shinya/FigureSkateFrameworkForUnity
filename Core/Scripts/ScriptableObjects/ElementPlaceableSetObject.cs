using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ElementPlaceableSet", menuName = "ScriptableObjects/FigureSkateFramework/ElementPlaceableSet")]
    public class ElementPlaceableSetObject : ScriptableObject
    {
        public ElementPlaceableSet data;
    }
}