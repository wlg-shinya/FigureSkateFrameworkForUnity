using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionContainAllPatternInFirst", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionContainAllPatternInFirst")]
    public class ProgramComponentConditionContainAllPatternInFirstObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionContainAllPatternInFirst data;
        public override ProgramComponentCondition Data() => data;
    }
}