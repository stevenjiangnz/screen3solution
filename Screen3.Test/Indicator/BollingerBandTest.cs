using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class BollingerBandTest
    {
        [Fact]
        public void TestBollingerBand()
        {
            double[] inData = new double[]{
                86.16, 89.09, 88.78, 90.32, 89.07,
                91.15, 89.44, 89.18, 86.93, 87.68,
                86.96, 89.43, 89.32, 88.72, 87.45,
                87.26, 89.50, 87.90, 89.13, 90.70,
                92.90, 92.98, 91.80, 92.66, 92.68,
                92.30, 92.77, 92.54, 92.95, 93.20,
                91.07, 89.83, 89.74, 90.40, 90.74,
                88.02, 88.09, 88.84, 90.78, 90.54,
                91.39, 90.65};

            int len = inData.Length;
            int period = 20;

            double?[] m = new double?[len];
            double?[] h = new double?[len];
            double?[] l = new double?[len];

            BollingerBand.Calculate(inData, period, 2.0, m, h, l);

            Console.WriteLine("middle: " + ObjectHelper.ToJson(m) + "\n");
            Console.WriteLine("high: " + ObjectHelper.ToJson(h) + "\n");
            Console.WriteLine("low: " + ObjectHelper.ToJson(l) + "\n");

        }
    }
}
