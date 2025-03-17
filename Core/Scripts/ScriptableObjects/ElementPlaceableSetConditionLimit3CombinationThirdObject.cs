using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ElementPlaceableSetConditionLimit3CombinationThird", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ElementPlaceableSetConditionLimit3CombinationThird")]
    public class ElementPlaceableSetConditionLimit3CombinationThirdObject : ElementPlaceableSetConditionObject
    {
        public ElementPlaceableSetConditionLimit3CombinationThird data;
        public override ElementPlaceableSetCondition Data() => data;
    }
}