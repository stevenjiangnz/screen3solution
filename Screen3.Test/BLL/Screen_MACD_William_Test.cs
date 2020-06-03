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
    public class Screen_MACD_William_Test
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";
        private Screen_MACD_William bll = null;
        public Screen_MACD_William_Test()
        {
            this.bll = new Screen_MACD_William(s3_bucket_name, tempTickerFolder);
        }

        [Fact]
        public async void TestRetrieveData()
        {
            await this.bll.RetrieveData("SUN");
        }


        [Fact]
        public async void TestGetEntryMatchTickers()
        {
            await this.bll.RetrieveData("SUN", 20180101, 0);
            var result = this.bll.GetEntryMatchTickers(new Dictionary<string, object>{
                {"WILLIAM_BUY_LEVEL", -80},
                {"WILLIAM_SELL_LEVEL", -20},
                {"DECLUSTER", 2},
                {"DIRECTION", "SELL"}
            });

            Console.WriteLine("result: " + ObjectHelper.ToJson(result));
        }

        [Fact]
        public async void TestGetEntryMatchTickersFromFactory()
        {
            var screenObj = ScreenFactory.GetScreenFunction("Screen_MACD_William", this.s3_bucket_name, this.tempTickerFolder);
            await screenObj.RetrieveData("SUN", 20180101, 0);
            var result = screenObj.GetEntryMatchTickers(new Dictionary<string, object>{
                {"WILLIAM_BUY_LEVEL", -80},
                {"WILLIAM_SELL_LEVEL", -20},
                {"DECLUSTER", 2},
                {"DIRECTION", "SELL"}
            });

            Console.WriteLine("result: " + ObjectHelper.ToJson(result));
        }

    }
}