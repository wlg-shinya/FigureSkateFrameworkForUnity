using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionContainAllPatternInFirst", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionContainAllPatternInFirst")]
    public class ProgramComponentConditionContainAllPatternInFirstObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionContainAllPatternInFirst data;
        public override ProgramComponentCondition Data() => data;
    }
}