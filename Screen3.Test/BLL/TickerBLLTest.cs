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
    public class TickerBLLTest
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        [Fact]
        public void TestSaveTickers() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);

            List<TickerEntity> tickerList = new List<TickerEntity>();

            tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20081201,
                Open = (float)66.01,
                High = (float)77.01,
                Low = (float)999.0,
                Close = (float)12.1,
                Volume = 123124
            });

            tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20071201,
                Open = (float)111111.01,
                High = (float)11111.01,
                Low = (float)111.0,
                Close = (float)111.1,
                Volume = 123124
            });

            tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20091212,
                Open = (float)33.01,
                High = (float)199.01,
                Low = (float)13.0,
                Close = (float)13.1,
                Volume = 2346
            });

            tickerList.Add(new TickerEntity {
                Code = "CLL",
                Period = 20081202,
                Open = (float)33.01,
                High = (float)99.01,
                Low = (float)13.0,
                Close = (float)13.1,
                Volume = 2346
            });

            bll.SaveTickersToS3("CLL", tickerList, true).Wait();
        }


        [Fact]
        public void TestGetExistingDayTickers() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);
            bll.GetExistingDayTickersFromS3("CLL").Wait();
        }

        [Fact]
        public void TestGetWeekListFromDay() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);
            string tickerFile = "/home/steven/devlocal/screen3solution/Fixture/ANZ_day_small.txt";

            string content = File.ReadAllText(tickerFile);

            List<TickerEntity> tickers =  bll.getTickerListFromString(content);

            foreach(var t in tickers ) {

                Console.WriteLine(DateHelper.ToDate(t.Period).ToLongDateString() + "  " + t.ToString());
            }

            List<TickerEntity> weeklyTickers = bll.GetWeeklyTickerListFromDayList(tickers);

            Console.Write("\n\n");
            foreach(var wt in weeklyTickers) {
                Console.WriteLine(DateHelper.ToDate(wt.Period).ToLongDateString() + "  " + wt.ToString());
            }

        }

        [Fact]
        public async void TestGetDailyTickerEntityList(){
            TickerBLL bll = new TickerBLL(this.s3_bucket_name);
            string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";

            await bll.GetDailyTickerEntityList("ANZ", tempTickerFolder);
            await bll.GetDailyTickerEntityList("SUN", tempTickerFolder);

        }
    }
}
