using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementPlaceableSet", menuName = "ScriptableObjects/FigureSkateFramework/ElementPlaceableSet")]
    public class ElementPlaceableSetObject : ScriptableObject
    {
        public ElementPlaceableSet data;
    }
}