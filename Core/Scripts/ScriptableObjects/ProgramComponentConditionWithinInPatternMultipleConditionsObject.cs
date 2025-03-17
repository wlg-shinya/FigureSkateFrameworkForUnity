using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionWithinInPatternMultipleConditions", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionWithinInPatternMultipleConditions")]
    public class ProgramComponentConditionWithinInPatternMultipleConditionsObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionWithinInPatternMultipleConditions data;
        public override ProgramComponentCondition Data() => data;
    }
}