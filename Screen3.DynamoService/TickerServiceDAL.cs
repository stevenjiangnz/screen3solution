using System;
using System.Threading.Tasks;
using Screen3.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;

using Screen3.Utils;

namespace Screen3.DynamoService
{
    public class TickerServiceDAL
    {
        private AmazonDynamoDBClient client;
        private Table table;

        public TickerServiceDAL(string tableName)
        {
            this.client = new AmazonDynamoDBClient();
            this.table = Table.LoadTable(this.client, tableName);
        }


        public async Task Delete(string code, int period)
        {

            var doc = await this.table.GetItemAsync(code, period);

            await this.table.DeleteItemAsync(doc);
        }

        public async Task<TickerEntity> GetItem(string code, int period)
        {
            var doc = await this.table.GetItemAsync(code, period);

            return this.toTickerEntity(doc);
        }

        public async Task<List<TickerEntity>> GetItemsByCode(string code)
        {
            List<TickerEntity> resultList = new List<TickerEntity>();
            QueryFilter filter = new QueryFilter();
            filter.AddCondition("code", QueryOperator.Equal, code);
            Search search = this.table.Query(filter);

            List<Document> documentSet = new List<Document>();
            do
            {
                documentSet = await search.GetNextSetAsync();
                foreach (var document in documentSet)
                {

                    resultList.Add(this.toTickerEntity(document));
                }
            } while (!search.IsDone);

            return resultList;
        }


        public async Task Update(TickerEntity ticker)
        {
            Document updatedAccountDoc = null;
            Document accountDoc = this.toTickerDocument(ticker);

            updatedAccountDoc = await this.table.UpdateItemAsync(accountDoc);

            return;
        }

        public async Task UpdateBatch(List<TickerEntity> tickers)
        {
            var batchWriter = this.table.CreateBatchWrite();

            foreach(TickerEntity tr in tickers) {
                Document tickerDoc = this.toTickerDocument(tr);
                batchWriter.AddDocumentToPut(tickerDoc);
            }

            await batchWriter.ExecuteAsync();
            return;
        }


        private Document toTickerDocument(TickerEntity ticker)
        {
            var doc = new Document();

            doc["code"] = ticker.T;
            doc["p"] = ticker.P;
            doc["o"] = ticker.O;
            doc["h"] = ticker.H;
            doc["c"] = ticker.C;
            doc["l"] = ticker.L;
            doc["v"] = ticker.V;

            return doc;
        }

        private TickerEntity toTickerEntity(Document doc)
        {
            TickerEntity en = null;

            if (doc != null)
            {
                en = new TickerEntity();
                en.T = doc["code"].ToString();
                en.P = doc["p"].AsInt();
                en.O = (float)doc["o"].AsDecimal();
                en.H = (float)doc["h"].AsDecimal();
                en.L = (float)doc["l"].AsDecimal();
                en.C = (float)doc["c"].AsDecimal();
                en.V = doc["v"].AsLong();
            }
            return en;
        }

    }
}

