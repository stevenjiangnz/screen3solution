using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class ATRTest
    {
        [Fact]
        public void TestATR()
        {
            double[] high = new double[]{
            48.70, 48.72, 48.90, 48.87, 48.82,
            49.05, 49.20, 49.35, 49.92, 50.19,
            50.12, 49.66, 49.88, 50.19, 50.36,
            50.57, 50.65, 50.43, 49.63, 50.33,
            50.29, 50.17, 49.32, 48.50, 48.32,
            46.80, 47.80, 48.39, 48.66, 48.79
            };

            double[] low = new double[]{
            47.79, 48.14, 48.39, 48.37, 48.24,
            48.64, 48.94, 48.86, 49.50, 49.87,
            49.20, 48.90, 49.43, 49.73, 49.26,
            50.09, 50.30, 49.21, 48.98, 49.61,
            49.20, 49.43, 48.08, 47.64, 41.55,
            44.28, 47.31, 47.20, 47.90, 47.73
            };


            double[] close = new double[]{
            48.16, 48.61, 48.75, 48.63, 48.74,
            49.03, 49.07, 49.32, 49.91, 50.13,
            49.53, 49.50, 49.75, 50.03, 50.31,
            50.52, 50.41, 49.34, 49.37, 50.23,
            49.24, 49.93, 48.43, 48.18, 46.57,
            45.41, 47.77, 47.72, 48.62, 47.85
            };

            int len = close.Length;

            double?[] outATR = new double?[len];


            ATR.Calculate(high, low, close, 14, outATR);

            Console.WriteLine("di plus: " + ObjectHelper.ToJson(outATR));
        }
    }
}
