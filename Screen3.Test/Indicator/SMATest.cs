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
            double?[] outputData;
            int len = inputData.Length;
            outputData = new double?[len];
            int period = 5;


            Result res = SMA.Calculate(inputData, period, outputData);

            Console.WriteLine(ObjectHelper.ToJson(outputData));

        }

    }
}
