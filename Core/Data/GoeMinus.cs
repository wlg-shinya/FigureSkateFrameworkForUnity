using System;

namespace Wlg.FigureSkate.Core.Data
{
    // Grade Of Execution の減点項目のいち要素
    [Serializable]
    public class GoeMinus
    {
        // チェックの説明
        public string description;
        // グループ名
        // 同名の項目が減点対象となったときそのうちの一つしか適用されなくなります
        // 空白だと未グループとしてこのルールは適用されません
        public string group;
        // 減点最小値(値的にはmaxValueより大きい点に注意)
        public int minValue;
        // 減点最大値(値的にはminValueより小さい点に注意)
        public int maxValue;
        // 基礎点倍率
        public float baseValueFactor = 1.0f;
        // ダウングレードとなるか
        public bool isDowngrade;
        // 判定時に付与される記号(存在しない場合は空白)
        public string mark;
        // 適用される構成要素ID。空の場合はすべての構成要素が対象となる
        public string[] targetElementIds;

        // 減点の合計値。この値が最も小さい要素（＝減点量が大きい要素）がグループの代表となる
        public int TotalValue() => minValue + maxValue;
    }
}