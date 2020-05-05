using System;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;

namespace Screen3.Test.DynamoService
{
    public class StockServiceDALTest
    {
        [Fact]
        public void TestInsertNewStock()
        {
            StockServiceDAL service = new StockServiceDAL("stevenjiangnz-screen3-db");

            StockEntity stock = new StockEntity{
                Code = "sun",
                Company = "suncorp",
                Sector = "sector 111",
                Cap = 123,
                Weight = 0.012
            };
            service.InsertNewStock(stock).Wait();
        }
    }
}
