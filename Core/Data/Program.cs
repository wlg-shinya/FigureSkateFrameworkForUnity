using System;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム（ショートプログラム、フリースケーティングなど）
    [Serializable]
    public class Program
    {
        // 名称
        public string name;
        // プログラム構成要素の規則
        public ProgramComponentRegulation regulation;
        // 後半ジャンプボーナス数
        public int lastJumpSpecialFactorCount;
        // 演技構成点倍率
        public float pcsFactor;
    }
}