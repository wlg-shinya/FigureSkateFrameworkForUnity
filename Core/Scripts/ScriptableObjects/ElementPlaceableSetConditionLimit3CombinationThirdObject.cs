using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementPlaceableSetConditionLimit3CombinationThird", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ElementPlaceableSetConditionLimit3CombinationThird")]
    public class ElementPlaceableSetConditionLimit3CombinationThirdObject : ElementPlaceableSetConditionObject
    {
        public ElementPlaceableSetConditionLimit3CombinationThird data;
        public override ElementPlaceableSetCondition Data() => data;
    }
}