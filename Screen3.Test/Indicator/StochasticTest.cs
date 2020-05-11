using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class StochasticTest
    {
        [Fact]
        public void TestStochastic()
        {
            double[] high = new double[]{
                127.01, 127.62, 126.59, 127.35, 128.17,
                128.43, 127.37, 126.42, 126.90, 126.85,
                125.65, 125.72, 127.16, 127.72, 127.69,
                128.22, 128.27, 128.09, 128.27, 127.74,
                128.77, 129.29, 130.06, 129.12, 129.29,
                128.47, 128.09, 128.65, 129.14, 128.64};

            double[] low = new double[]{
                125.36, 126.16, 124.93, 126.09, 126.82,
                126.48, 126.03, 124.83, 126.39, 125.72,
                124.56, 124.57, 125.07, 126.86, 126.63,
                126.80, 126.71, 126.80, 126.13, 125.92,
                126.99, 127.81, 128.47, 128.06, 127.61,
                127.60, 127.00, 126.90, 127.49, 127.40};

            double[] close = new double[]{
                0, 0, 0, 0, 0,
                0, 0, 0, 0, 0,
                0, 0, 0, 127.29, 127.18,
                128.01, 127.11, 127.73, 127.06, 127.33,
                128.71, 127.87, 128.58, 128.60, 127.93,
                128.11, 127.60, 127.60, 128.69, 128.27};

            double?[] k = new double?[close.Length];
            double?[] d = new double?[close.Length];


            Stochastic.CalculateSlow(close, high, low, 14, 3, k, d);

            // Console.WriteLine("stockastic k:" + ObjectHelper.ToJson(k));
            // Console.WriteLine("stockastic d:" + ObjectHelper.ToJson(d));

        }

    }
}
