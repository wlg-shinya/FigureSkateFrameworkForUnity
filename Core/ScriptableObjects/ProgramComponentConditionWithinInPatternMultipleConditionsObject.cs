using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionWithinInPatternMultipleConditions", menuName = "ScriptableObjects/ProgramComponentCondition/ProgramComponentConditionWithinInPatternMultipleConditions")]
    public class ProgramComponentConditionWithinInPatternMultipleConditionsObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionWithinInPatternMultipleConditions data;
        public override ProgramComponentCondition Data() => data;
    }
}