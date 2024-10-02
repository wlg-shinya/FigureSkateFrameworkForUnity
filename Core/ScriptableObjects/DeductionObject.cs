using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Deduction", menuName = "ScriptableObjects/Deduction")]
    public class DeductionObject : ScriptableObject
    {
        public Deduction data;
    }
}