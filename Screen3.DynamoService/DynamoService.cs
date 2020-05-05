using System;
using System.Threading.Tasks;
using Screen3.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace Screen3.DynamoService
{
    public class DynamoService
    {
        private AmazonDynamoDBClient client;
        private Table table;
        
        public DynamoService(string tableName) {
            this.client = new AmazonDynamoDBClient();
            this.table = Table.LoadTable(this.client, tableName);
        }
        
        public async Task InsertNewStock(StockEntity stock) {

        }
    }
}

