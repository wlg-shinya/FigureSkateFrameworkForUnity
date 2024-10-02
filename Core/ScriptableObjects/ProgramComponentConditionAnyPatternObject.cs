using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionAnyPattern", menuName = "ScriptableObjects/ProgramComponentCondition/ProgramComponentConditionAnyPattern")]
    public class ProgramComponentConditionAnyPatternObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionAnyPattern data;
        public override ProgramComponentCondition Data() => data;
    }
}