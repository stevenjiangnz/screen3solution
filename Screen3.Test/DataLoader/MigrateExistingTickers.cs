using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Test.DataLoader
{
    public class MigrateExistingTickers
    {
        private string s3_bucket_name = "stevenjiangnz-screen3-eod-source";
        private string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";

        [Fact]
        public async void TestMigrateExistingTickersToDb()
        {
            string tempTickerFolder = "/tmp/screen3_temp_files/localticker/";
            string tableName = "stevenjiangnz-screen3-tickers";
            TickerBLL bll = new TickerBLL(this.s3_bucket_name, tempTickerFolder);
            TickerDbBLL dbBll = new TickerDbBLL(tableName);
            string stock_tableName = "stevenjiangnz-screen3-asx300";

            StockBLL stockbll = new StockBLL(stock_tableName);
            var stockList = await stockbll.GetAll();

            Console.WriteLine("stocklist: " + ObjectHelper.ToJson(stockList.Count));

            foreach (var stock in stockList)
            {
                var tickerList = await bll.GetDailyTickerEntityList(stock.Code);

                await dbBll.AppendTickerRange(tickerList);
                Console.WriteLine($"updated ticker count: {tickerList.Count}, {stock.Code}");

            }


        }
    }
}
