using System;
using System.Collections.Generic;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;
using Screen3.BLL;
using Screen3.Entity;

namespace Screen3.Test.Indicator
{
    public class MACDTest
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";


        [Fact]
        public async void TestMACD()
        {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, this.tempTickerFolder);
            List<TickerEntity> tList = await bll.GetDailyTickerEntityList("ORG", 20181001, 20191101);

            double[] inputData = new double[tList.Count];
            
            var i = 0;
            foreach (var t in tList)
            {
                inputData[i] = t.C;
                i++;
            }

            double?[] m = new double?[tList.Count];
            double?[] s = new double?[tList.Count];
            double?[] h = new double?[tList.Count];

            Result res = MACD.Calculate(inputData, 26, 12, 9, m, s, h);

            var tArray = tList.ToArray();
            for (int j = 0; j< tList.Count; j++) {
                Console.WriteLine($"{tArray[j].T} {tArray[j].P} O: {tArray[j].O} H: {tArray[j].H} L: {tArray[j].L} C: {tArray[j].C} M: {m[j]} S: {s[j]} H: {h[j]} ");
            }
        }

    }
}
