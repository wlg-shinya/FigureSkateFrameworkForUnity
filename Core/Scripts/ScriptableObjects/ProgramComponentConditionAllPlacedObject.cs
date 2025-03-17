using UnityEngine;

namespace Wlg.FigureSkate.Core
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionAllPlaced", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentCondition/ProgramComponentConditionAllPlaced")]
    public class ProgramComponentConditionAllPlacedObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionAllPlaced data;
        public override ProgramComponentCondition Data() => data;
    }
}