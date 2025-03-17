using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionAnyPattern", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionAnyPattern")]
    public class ProgramComponentConditionAnyPatternObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionAnyPattern data;
        public override ProgramComponentCondition Data() => data;
    }
}