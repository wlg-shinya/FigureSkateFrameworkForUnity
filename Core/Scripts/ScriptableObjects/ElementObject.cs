using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Element", menuName = "ScriptableObjects/Element")]
    public class ElementObject : ScriptableObject
    {
        public Element data;
        // 対象GOE
        public GoeObject goeObject;
        // 一つ上のグレードの構成要素。null = 最高グレード
        public ElementObject upgrade = null;
        // 一つ下のグレードの構成要素。null = 最低グレード
        public ElementObject downgrade = null;
    }
}