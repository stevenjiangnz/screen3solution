using System;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;

namespace Screen3.Test.DynamoService
{
    public class StockServiceDALTest
    {
        private string tableName = "stevenjiangnz-screen3-asx300";
        [Fact]
        public void TestInsertNewStock()
        {
            StockServiceDAL service = new StockServiceDAL(this.tableName);

            StockEntity stock = new StockEntity{
                Code = "sun",
                Company = "suncorp",
                Sector = "sector 111",
                Cap = 123,
                Weight = 0.012
            };
            service.InsertNewStock(stock).Wait();
        }

        [Fact]
        public async void TestGetAll()
        {
            StockServiceDAL service = new StockServiceDAL(this.tableName);

            var stocklist = await service.GetAll();

            Console.WriteLine("list count: " +  stocklist.Count);
        }

        [Fact]
        public async void TestDelete()
        {
            StockServiceDAL service = new StockServiceDAL(this.tableName);

            await service.Delete("SUN");

        }

        [Fact]
        public async void TestGetItem()
        {
            StockServiceDAL service = new StockServiceDAL(this.tableName);

            var stock = await service.GetItem("CCL");

            Console.WriteLine("code : " +  stock.Code + " " + stock.Company);
        }
    }
}
