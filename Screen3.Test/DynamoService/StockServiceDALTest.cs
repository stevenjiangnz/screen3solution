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

    }
}
