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
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";

        [Fact]
        public void TestSaveTickers() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, this.tempTickerFolder);

            List<TickerEntity> tickerList = new List<TickerEntity>();

            tickerList.Add(new TickerEntity {
                T = "CLL",
                P = 20081201,
                O = (float)66.01,
                H = (float)77.01,
                L = (float)999.0,
                C = (float)12.1,
                V = 123124
            });

            tickerList.Add(new TickerEntity {
                T = "CLL",
                P = 20071201,
                O = (float)111111.01,
                H = (float)11111.01,
                L = (float)111.0,
                C = (float)111.1,
                V = 123124
            });

            tickerList.Add(new TickerEntity {
                T = "CLL",
                P = 20091212,
                O = (float)33.01,
                H = (float)199.01,
                L = (float)13.0,
                C = (float)13.1,
                V = 2346
            });

            tickerList.Add(new TickerEntity {
                T = "CLL",
                P = 20081202,
                O = (float)33.01,
                H = (float)99.01,
                L = (float)13.0,
                C = (float)13.1,
                V = 2346
            });

            bll.SaveTickersToS3("CLL", tickerList, true).Wait();
        }


        [Fact]
        public void TestGetExistingDayTickers() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, this.tempTickerFolder);
            bll.GetExistingDayTickersFromS3("CLL").Wait();
        }

        [Fact]
        public void TestGetWeekListFromDay() {
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, this.tempTickerFolder);
            string tickerFile = "/home/steven/devlocal/screen3solution/Fixture/ANZ_day_small.txt";

            string content = File.ReadAllText(tickerFile);

            List<TickerEntity> tickers =  bll.getTickerListFromString(content);

            foreach(var t in tickers ) {

                Console.WriteLine(DateHelper.ToDate(t.P).ToLongDateString() + "  " + t.ToString());
            }

            List<TickerEntity> weeklyTickers = bll.GetWeeklyTickerListFromDayList(tickers);

            Console.Write("\n\n");
            foreach(var wt in weeklyTickers) {
                Console.WriteLine(DateHelper.ToDate(wt.P).ToLongDateString() + "  " + wt.ToString());
            }

        }

        [Fact]
        public async void TestGetDailyTickerEntityList(){
            string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, tempTickerFolder);

            await bll.GetDailyTickerEntityList("ANZ");
            await bll.GetDailyTickerEntityList("SUN");

        }
    }
}
