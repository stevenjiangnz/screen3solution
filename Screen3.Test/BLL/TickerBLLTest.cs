using System;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
namespace Screen3.Test.BLL
{
    public class TickerBLLTest
    {
        [Fact]
        public void TestSaveTickers() {
            TickerBLL bll = new TickerBLL();

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
                Period = 20081201,
                Open = (float)111111.01,
                High = (float)11111.01,
                Low = (float)111.0,
                Close = (float)111.1,
                Volume = 123124
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

            bll.SaveTickers("CLL", tickerList).Wait();
        }


        [Fact]
        public void TestGetExistingDayTickers() {
            TickerBLL bll = new TickerBLL();
            bll.GetExistingDayTickers("CLL").Wait();
        }
    }
}
