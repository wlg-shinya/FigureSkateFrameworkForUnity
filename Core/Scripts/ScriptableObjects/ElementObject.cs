using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/FigureSkateFramework/Element")]
    public class ElementObject : ScriptableObject
    {
        public Element data;
    }
}