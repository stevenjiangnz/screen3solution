namespace Screen3.BLL
{
    public class ScreenFactory
    {
        public static IScreenInterface GetScreenFunction(string name, string s3_bucket_name, string localFolder)
        {
            IScreenInterface screenObject = null;
            switch (name)
            {
                case "Screen_MACD_William":
                    screenObject = new Screen_MACD_William(s3_bucket_name, localFolder);
                    break;
                case "Screen_ADX":
                    screenObject = new Screen_ADX(s3_bucket_name, localFolder);
                    break;

            }

            return screenObject;
        }
    }
}