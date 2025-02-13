using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/FigureSkateFramework/Element")]
    public class ElementObject : ScriptableObject
    {
        public Element data;
    }
}