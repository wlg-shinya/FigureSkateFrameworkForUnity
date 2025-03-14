namespace Wlg.FigureSkate.Fact.External
{
    // https://koma2961.hatenablog.com/entry/2016/02/24/051156
    public class StringWidthConverter
    {
        const int ConvertionConstant = 65248;

        static public string ConvertToFullWidth(string halfWidthStr)
        {
            string fullWidthStr = null;

            for (int i = 0; i < halfWidthStr.Length; i++)
            {
                fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
            }

            return fullWidthStr;
        }

        static public string ConvertToHalfWidth(string fullWidthStr)
        {
            string halfWidthStr = null;

            for (int i = 0; i < fullWidthStr.Length; i++)
            {
                halfWidthStr += (char)(fullWidthStr[i] - ConvertionConstant);
            }

            return halfWidthStr;
        }
    }
}