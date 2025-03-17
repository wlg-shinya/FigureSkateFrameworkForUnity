using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ElementPlaceableSetConditionInvalid2Combination", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ElementPlaceableSetConditionInvalid2Combination")]
    public class ElementPlaceableSetConditionInvalid2CombinationObject : ElementPlaceableSetConditionObject
    {
        public ElementPlaceableSetConditionInvalid2Combination data;
        public override ElementPlaceableSetCondition Data() => data;
    }
}