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
    public class Screen_ADX_Test
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";
        private Screen_ADX bll = null;
        public Screen_ADX_Test()
        {
            this.bll = new Screen_ADX(s3_bucket_name, tempTickerFolder);
        }

        [Fact]
        public async void TestDoScreen_ADX()
        {
            var result = await this.bll.DoScreen("RIO", "day", 20100101, 20200101, new Dictionary<string, object>{
                {"OFFSET", 3},
                {"DIRECTION", "SELL"}
            });

            Console.WriteLine("result: " + ObjectHelper.ToJson(result));
            Console.WriteLine($"result {result.Count}");

        }


        [Fact]
        public async void TestGetEntryMatchTickersFromFactory_ADX()
        {
            var screenObj = ScreenFactory.GetScreenFunction("Screen_ADX", this.s3_bucket_name, this.tempTickerFolder);
            await screenObj.DoScreen("SUN");
            var result = screenObj.GetEntryMatchTickers(new Dictionary<string, object>{
                {"OFFSET", 5},
                {"DIRECTION", "SELL"}
            });

            Console.WriteLine("result: " + ObjectHelper.ToJson(result));
        }

    }
}