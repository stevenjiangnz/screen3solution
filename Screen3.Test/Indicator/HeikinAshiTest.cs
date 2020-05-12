using System;
using System.Collections.Generic;
using Xunit;
using Screen3.Indicator;
using Screen3.Utils;
using Screen3.BLL;
using Screen3.Entity;

namespace Screen3.Test.Indicator
{
    public class HeikinAshiTest
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";


        [Fact]
        public async void TestHeikinAshi()
        {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, this.tempTickerFolder);
            List<TickerEntity> tList = await bll.GetDailyTickerEntityList("ORG", 20191001, 20191101);
            
            double[] o = new double[tList.Count];
            double[] h = new double[tList.Count];
            double[] l = new double[tList.Count];
            double[] c = new double[tList.Count];

            double?[] oo = new double?[tList.Count];
            double?[] oh = new double?[tList.Count];
            double?[] ol = new double?[tList.Count];
            double?[] oc = new double?[tList.Count];

            var i = 0;
            foreach (var t in tList)
            {
                o[i] = t.O;
                h[i] = t.H;
                l[i] = t.L;
                c[i] = t.C;

                i++;
            }

            Result res = HeikinAshi.Calculate(o, c, h, l, oo, oc, oh, ol);

            Console.WriteLine(ObjectHelper.ToJson(oo));
        }

    }
}
