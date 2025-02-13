using System.Collections.Generic;
using UnityEngine;
using Wlg.FigureSkate.Core.Data;

namespace Wlg.FigureSkate.Core.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Competition", menuName = "ScriptableObjects/FigureSkateFramework/Competition")]
    public class CompetitionObject : ScriptableObject
    {
        public Competition data;
    }
}