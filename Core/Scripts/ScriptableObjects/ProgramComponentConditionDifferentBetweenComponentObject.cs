using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionDifferentBetweenComposition", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionDifferentBetweenComposition")]
    public class ProgramComponentConditionDifferentBetweenComponentObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionDifferentBetweenComponent data;
        public override ProgramComponentCondition Data() => data;
    }
}