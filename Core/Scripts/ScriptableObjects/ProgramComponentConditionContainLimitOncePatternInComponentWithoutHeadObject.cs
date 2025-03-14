using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead")]
    public class ProgramComponentConditionContainLimitOncePatternInComponentWithoutHeadObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionContainLimitOncePatternInComponentWithoutHead data;
        public override ProgramComponentCondition Data() => data;
    }
}