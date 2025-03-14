using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionDifferentBetweenComposition", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionDifferentBetweenComposition")]
    public class ProgramComponentConditionDifferentBetweenComponentObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionDifferentBetweenComponent data;
        public override ProgramComponentCondition Data() => data;
    }
}