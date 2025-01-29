using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentRegulation", menuName = "ScriptableObjects/ProgramComponentRegulation")]
    public class ProgramComponentRegulationObject : ScriptableObject
    {
        public ProgramComponentRegulation data;
    }
}