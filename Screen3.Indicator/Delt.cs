using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen3.Indicator
{
    public class Delt
    {

        public static Result Calculate(double?[] inputData, int offset, double?[] outData)
        {
            Result result = new Result();
            result.Status = ResultStatus.Success;

            int len = inputData.Length;

            try
            {
                for (int i = offset; i < len; i++)
                {
                    if (inputData[i - offset].HasValue && inputData[i].HasValue && inputData[i].Value != 0)
                    {
                        outData[i] = Math.Round(100 * (inputData[i].Value - inputData[i - offset].Value) / inputData[i].Value, 4);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = ResultStatus.Fail;
                result.Message = ex.ToString();
            }

            return result;
        }
    }
}