using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentConditionAllPlaced", menuName = "ScriptableObjects/ProgramComponentCondition/ProgramComponentConditionAllPlaced")]
    public class ProgramComponentConditionAllPlacedObject : ProgramComponentConditionObject
    {
        public ProgramComponentConditionAllPlaced data;
        public override ProgramComponentCondition Data() => data;
    }
}