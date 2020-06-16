using System;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.Utils;

namespace Screen3.Test.DynamoService
{
    public class TickerServiceDALTest
    {
        private string tableName = "stevenjiangnz-screen3-tickers";
        private string id = "SUN";

        [Fact]
        public async void TestInsertNewTickerCollection()
        {
            TickerServiceDAL tradeService = new TickerServiceDAL(tableName);

            await tradeService.InsertNewTrade(new TickerCollectionEntity
            {
                Code = this.id,
                Tickers = new List<TickerEntity>{
                    new TickerEntity{ T = this.id, P= 20120103, O= (float)1.1, H=(float)1.5, L= (float)0.9, C = (float)0.9, V=1212321},
                    new TickerEntity{ T = this.id, P= 20120104, O= (float)2.1, H=(float)2.5, L= (float)1.9, C = (float)1.9, V=2343221},
                }
            });

            this.id = "RIO";
            await tradeService.InsertNewTrade(new TickerCollectionEntity
            {
                Code = this.id,
                Tickers = new List<TickerEntity>{
                    new TickerEntity{ T = this.id, P= 20120103, O= (float)1.1, H=(float)1.5, L= (float)0.9, C = (float)0.9, V=1212321},
                    new TickerEntity{ T = this.id, P= 20120104, O= (float)2.1, H=(float)2.5, L= (float)1.9, C = (float)1.9, V=2343221},
                }
            });
        }

        [Fact]
        public async void TestGetAll_TickerCollection()
        {
            TickerServiceDAL service = new TickerServiceDAL(this.tableName);

            var tickerList = await service.GetAll();

            Console.WriteLine("list count: " + tickerList.Count + ObjectHelper.ToJson(tickerList));
        }

        [Fact]
        public async void TestGetItem_TickerCollection()
        {
            TickerServiceDAL service = new TickerServiceDAL(this.tableName);

            var accountlist = await service.GetItem(this.id);

            Console.WriteLine("list item details: " + ObjectHelper.ToJson(accountlist));
        }

        [Fact]
        public async void TestDelete_TickerCollection()
        {
            TradeServiceDAL service = new TradeServiceDAL(this.tableName);

            await service.Delete(this.id);

        }


        [Fact]
        public async void TestUpdate_TickerCollection()
        {
            TickerServiceDAL service = new TickerServiceDAL(this.tableName);

            TickerCollectionEntity updateAccount = await service.Update(new TickerCollectionEntity
            {
                Code = this.id,
                Tickers = new List<TickerEntity>{
                    new TickerEntity{ T = this.id, P= 20120103, O= (float)111.1, H=(float)1.5, L= (float)0.9, C = (float)0.9, V=1212321},
                    new TickerEntity{ T = this.id, P= 20120104, O= (float)1112.1, H=(float)2.5, L= (float)1.9, C = (float)1.9, V=2343221},
                }
            });
            this.id = "RIO";
            updateAccount = await service.Update(new TickerCollectionEntity
            {
                Code = this.id,
                Tickers = new List<TickerEntity>{
                    new TickerEntity{ T = this.id, P= 20120103, O= (float)1.1, H=(float)1.5, L= (float)0.9, C = (float)0.9, V=1212321},
                    new TickerEntity{ T = this.id, P= 20120104, O= (float)2.1, H=(float)2.5, L= (float)1.9, C = (float)1.9, V=2343221},
                }
            });
        }

    }
}