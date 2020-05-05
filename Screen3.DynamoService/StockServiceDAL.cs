using System;
using System.Threading.Tasks;
using Screen3.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace Screen3.DynamoService
{
    public class StockServiceDAL
    {
        private AmazonDynamoDBClient client;
        private Table table;
        
        public StockServiceDAL(string tableName) {
            this.client = new AmazonDynamoDBClient();
            this.table = Table.LoadTable(this.client, tableName);
        }
        
        public async Task InsertNewStock(StockEntity stock) {
            var doc = new Document();

            doc["code"] = stock.Code;
            doc["company"] = stock.Company;
            doc["sector"] = stock.Sector;
            doc["cap"] = stock.Cap;
            doc["weight"] = stock.Weight;

            await this.table.PutItemAsync(doc);
        }
    }
}

