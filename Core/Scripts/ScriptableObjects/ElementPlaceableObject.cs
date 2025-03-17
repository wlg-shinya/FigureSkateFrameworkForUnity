using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ElementPlaceable", menuName = "ScriptableObjects/FigureSkateFramework/ElementPlaceable")]
    public class ElementPlaceableObject : ScriptableObject
    {
        public ElementPlaceable data;
    }
}