using System;
using System.Threading.Tasks;
using Screen3.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;

using Screen3.Utils;

namespace Screen3.DynamoService
{
    public class StockServiceDAL
    {
        private AmazonDynamoDBClient client;
        private Table table;

        public StockServiceDAL(string tableName)
        {
            this.client = new AmazonDynamoDBClient();
            this.table = Table.LoadTable(this.client, tableName);
        }

        public async Task InsertNewStock(StockEntity stock)
        {
            var doc = new Document();

            doc["code"] = stock.Code;
            doc["company"] = stock.Company;
            doc["sector"] = stock.Sector;
            doc["cap"] = stock.Cap;
            doc["weight"] = stock.Weight;

            await this.table.PutItemAsync(doc);
        }

        public async Task<List<StockEntity>> GetAll()
        {
            List<StockEntity> stockList = new List<StockEntity>();
            ScanFilter scanFilter = new ScanFilter();
            Search getAllItems = this.table.Scan(scanFilter);

            List<Document> allItems = await getAllItems.GetRemainingAsync();

            foreach (Document doc in allItems)
            {
                StockEntity en = this.toEntity(doc);
                stockList.Add(en);
            }

            return stockList;
        }

        public async Task Delete(string code)
        {
            List<StockEntity> stockList = new List<StockEntity>();
            DeleteItemOperationConfig config = new DeleteItemOperationConfig
            {
                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };

            await this.table.DeleteItemAsync(code, config);

        }

        public async Task<StockEntity> GetItem(string code)
        {
            StockEntity stock = null;
            
            var doc = await this.table.GetItemAsync(code);
            stock = this.toEntity(doc);

            return stock;
        }
        private StockEntity toEntity(Document doc)
        {
            StockEntity en = null;

            if (doc != null)
            {
                en = new StockEntity();
                en.Code = doc["code"].ToString();
                en.Company = doc["company"].ToString();
                en.Sector = doc.ContainsKey("sector") ? doc["sector"].ToString() : "";
                en.Cap = doc["cap"].AsDouble();
                en.Weight = doc["weight"].AsDouble();
            }

            return en;
        }
    }
}

