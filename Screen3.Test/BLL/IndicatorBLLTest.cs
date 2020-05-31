using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Test.BLL
{
    public class IndicatorBLLTest
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";

        [Fact]
        public async void TestGetSMA()
        {
            IndicatorBLL indBll = new IndicatorBLL(this.s3_bucket_name, this.tempTickerFolder);

            var smaResult = await indBll.GetSMA("CCL", 20, 20100101);

            Console.WriteLine(ObjectHelper.ToJson(smaResult));
        }

        [Fact]
        public async void TestGetEMA()
        {
            IndicatorBLL indBll = new IndicatorBLL(this.s3_bucket_name, this.tempTickerFolder);

            var smaResult = await indBll.GetEMA("CCL", 20, 2, 20100101);

            Console.WriteLine(ObjectHelper.ToJson(smaResult));
        }
    }
}