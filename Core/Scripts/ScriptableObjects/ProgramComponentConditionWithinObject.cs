using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionWithin", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionWithin")]
    public class ProgramComponentConditionWithinObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionWithin data;
        public override ProgramComponentCondition Data() => data;
    }
}