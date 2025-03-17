using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionDifferentPattern", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionDifferentPattern")]
    public class ProgramComponentConditionDifferentPatternObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionDifferentPattern data;
        public override ProgramComponentCondition Data() => data;
    }
}