using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ElementPlaceableSetConditionInvalid2Combination", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ElementPlaceableSetConditionInvalid2Combination")]
    public class ElementPlaceableSetConditionInvalid2CombinationObject : ElementPlaceableSetConditionObject
    {
        public ElementPlaceableSetConditionInvalid2Combination data;
        public override ElementPlaceableSetCondition Data() => data;
    }
}