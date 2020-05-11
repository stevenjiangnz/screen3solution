using System;


namespace Screen3.Indicator
{
    public class HeikinAshi
    {
        public static Result Calculate(double[] inOpen, double[] inClose, double[] inHigh, double[] inLow,
            double?[] outOpen, double?[] outClose, double?[] outHigh, double?[] outLow)
        {
            Result result = new Result();
            result.Status = ResultStatus.Success;

            int len = inOpen.Length;

            outOpen[0] = (inOpen[0] + inClose[0]) / 2;
            outClose[0] = (inOpen[0] + inClose[0] + inHigh[0] + inLow[0]) / 4;
            outHigh[0] = inHigh[0];
            outLow[0] = inLow[0];

            for(int i = 1; i < len; i++)
            {
                outClose[i] = (inOpen[i] + inHigh[i] + inLow[i] + inClose[i]) / 4;
                outOpen[i] = (outOpen[i-1] + outClose[i-1]) / 2;

                var h2 = (outClose[i] > outOpen[i]) ? outClose[i] : outOpen[i];
                outHigh[i] = (h2 > inHigh[i]) ? h2 : inHigh[i];

                var l2 = (outClose[i] < outOpen[i]) ? outClose[i] : outOpen[i];
                outLow[i] = (l2 < inLow[i]) ? l2 : inLow[i];
            }


            return result;
        }
    }
}