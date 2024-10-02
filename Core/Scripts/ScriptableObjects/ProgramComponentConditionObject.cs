using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    public abstract class ProgramComponentConditionObject : ScriptableObject
    {
        public abstract ProgramComponentCondition Data();
    }
}