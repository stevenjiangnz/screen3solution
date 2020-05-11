using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen3.Indicator
{
    public class BollingerBand
    {
        public static Result Calculate(double[] inputData, int period, double factor, double?[] outMiddle, double?[] outHigh, double?[] outLow)
        {
            Result res = new Result();
            res.Status = ResultStatus.Success;

            int len = inputData.Length;

            double[] sd = new double[len];

            try
            {
                SMA.Calculate(inputData, period, outMiddle);

                for (int i = period - 1; i < len; i++)
                {
                    double[] d = new double[period];

                    for (int j = 0; j < period; j++)
                    {
                        int bIndex = i + 1 - period;

                        d[j] = inputData[bIndex + j];
                    }

                    sd[i] = GenericHelper.StandardDeviation(d);

                    outHigh[i] = outMiddle[i] + factor * sd[i];

                    outLow[i] = outMiddle[i] - factor * sd[i];

                } }
            catch (Exception ex)
            {
                res.Status = ResultStatus.Fail;
                res.Message = ex.ToString();
                return res;
            }

            return res;
        }
    }
}