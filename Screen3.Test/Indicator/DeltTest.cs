using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class DeltTest
    {
        [Fact]
        public void TestDelt()
        {
            double[] inputData = new double[] { 22.27, 22.19, 22.08, 22.17, 22.18,
                22.13, 22.23, 22.43, 22.24, 22.29,
                22.15, 22.39, 22.38, 22.61, 23.36,
                24.05, 23.75, 23.83, 23.95, 23.63,
                23.82, 23.87, 23.65, 23.19, 23.10,
                23.33, 22.68, 23.10, 22.40, 22.17};

            int len = inputData.Length;
            double?[] outData = new double?[len];

            int period = 1;

            Delt.Calculate(inputData, period, outData);

            Console.WriteLine("result: " + ObjectHelper.ToJson(outData));
        }
    }
}
