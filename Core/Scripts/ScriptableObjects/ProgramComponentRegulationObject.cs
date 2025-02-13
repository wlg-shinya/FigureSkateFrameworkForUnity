using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProgramComponentRegulation", menuName = "ScriptableObjects/FigureSkateFramework/ProgramComponentRegulation")]
    public class ProgramComponentRegulationObject : ScriptableObject
    {
        public ProgramComponentRegulation data;
    }
}