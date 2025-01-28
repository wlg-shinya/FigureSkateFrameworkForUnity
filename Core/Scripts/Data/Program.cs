using System;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム（ショートプログラム、フリースケーティングなど）
    [Serializable]
    public class Program
    {
        // 一意の記号
        public string id;
        // 名称
        public string name;
        // 後半ジャンプボーナス数
        public int lastJumpSpecialFactorCount;
        // 演技構成点倍率
        public float pcsFactor;

        // プログラム構成要素の規則
        public ProgramComponentRegulation regulation;
    }
}