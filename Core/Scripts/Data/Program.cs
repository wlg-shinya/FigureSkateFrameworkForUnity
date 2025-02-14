using System;

namespace Wlg.FigureSkate.Core.Data
{
    // プログラム（シニア男子ショートプログラム、ジュニア女子フリースケーティングなど）
    [Serializable]
    public class Program
    {
        // 一意の記号
        public string id;
        // 正式名称
        public string name;
        // 性別とクラスを省いた名称
        public string shortName;
        // 後半ジャンプボーナス数
        public int lastJumpSpecialFactorCount;
        // 演技構成点倍率
        public float pcsFactor;
        // プログラム構成要素の規則ID
        public string programComponentRegulationId;
    }
}