using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionDifferentByComposition", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionDifferentByComposition")]
    public class ProgramComponentConditionDifferentByComponentObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionDifferentByComponent data;
        public override ProgramComponentCondition Data() => data;
    }
}