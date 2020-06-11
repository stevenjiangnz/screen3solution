using System;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.Utils;

namespace Screen3.Test.DynamoService
{
    public class TradeServiceDALTest
    {
        private string tableName = "stevenjiangnz-screen3-trade-db";
        private string id = "sadfdsfsdfsd";
        [Fact]
        public async void TestInsertNewAccount()
        {
            TradeServiceDAL tradeService = new TradeServiceDAL(tableName);

            await tradeService.InsertNewTrade(new AccountEntity
            {
                Id = this.id,
                Name = "test account nae",
                Trades = new List<TradeEntity>{
                    new TradeEntity{ Code = "sub", EntryPrice= 123456, EntryDate=123412321},
                    new TradeEntity{ Code = "rio", EntryPrice= 654321, EntryDate=1, ExitPrice=1.5, ExitDate= 123, PL = 0.9},
                }
            });
        }

        [Fact]
        public async void TestGetAll_Accounts()
        {
            TradeServiceDAL service = new TradeServiceDAL(this.tableName);

            var accountlist = await service.GetAll();

            Console.WriteLine("list count: " + accountlist.Count + ObjectHelper.ToJson(accountlist));
        }

                [Fact]
        public async void TestGetItem_Account()
        {
            TradeServiceDAL service = new TradeServiceDAL(this.tableName);

            var accountlist = await service.GetItem(this.id);

            Console.WriteLine("list item details: " + ObjectHelper.ToJson(accountlist));
        }

    }
}