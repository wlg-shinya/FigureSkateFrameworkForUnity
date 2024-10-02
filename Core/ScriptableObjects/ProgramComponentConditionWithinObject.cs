using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionWithin", menuName = "ScriptableObjects/ProgramComponentCondition/ProgramComponentConditionWithin")]
    public class ProgramComponentConditionWithinObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionWithin data;
        public override ProgramComponentCondition Data() => data;
    }
}