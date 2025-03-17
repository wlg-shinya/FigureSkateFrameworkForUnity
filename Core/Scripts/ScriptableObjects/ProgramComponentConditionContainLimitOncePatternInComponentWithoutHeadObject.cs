using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead")]
    public class ProgramComponentConditionContainLimitOncePatternInComponentWithoutHeadObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead data;
        public override ProgramComponentCondition Data() => data;
    }
}