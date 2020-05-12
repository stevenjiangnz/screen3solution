using System;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;

namespace Screen3.Test.Indicator
{
    public class RSITest
    {
        [Fact]
        public void TestRSI()
        {
             double[] close = new double[]{
                44.34, 44.09, 44.15, 43.61, 44.33,
                44.83, 45.10, 45.42, 45.84, 46.08,
                45.89, 46.03, 45.61, 46.28, 46.28,
                46.00, 46.03, 46.41, 46.22, 45.64,
                46.21, 46.25, 45.71, 46.45, 45.78,
                45.35, 44.03, 44.18, 44.22, 44.57,
                43.42, 42.66, 43.13
            };

            double?[] outRSI = new double?[close.Length];

            RSI.Calculate(close, 14, outRSI);

            Console.WriteLine(ObjectHelper.ToJson(outRSI));

        }

    }
}
