namespace Wlg.FigureSkate.Fact
{
    public static class FactConstant
    {
        // このフレームワークが対応しているシーズン
        public static readonly string[] SEASONS = new[]
        {
            "2022-23",
            "2023-24",
            "2024-25",
            "2025-26",
        };

        // 選手一人のプログラム構成の最大数
        public const int PLAYER_PROGRAM_COMPONENTS_MAX_COUNT = 2;
    }
}
