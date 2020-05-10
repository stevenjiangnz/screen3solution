using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class SMATest
    {
        [Fact]
        public void TestSMA()
        {
             double[] inputData = new double[] { 11, 12, 13, 14, 15, 16, 17 };
            int len = inputData.Length;

            int period = 5;

            var input = new SMAIn();
            input.Data = new SMAItem[inputData.Length];

            for(int i=0; i <inputData.Length; i++)
            {
                input.Data[i] = new SMAItem();
                input.Data[i].iClose = inputData[i];
            }

            var setting = new SMASetting();
            setting.Period = period;
            setting.Offset = 30;

            Result res = new SMA().Calculate(input, setting);

            Console.WriteLine(ObjectHelper.ToJson(input));
           
        }

    }
}
