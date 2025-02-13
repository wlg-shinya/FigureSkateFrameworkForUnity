using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionDifferentPattern", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionDifferentPattern")]
    public class ProgramComponentConditionDifferentPatternObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionDifferentPattern data;
        public override ProgramComponentCondition Data() => data;
    }
}