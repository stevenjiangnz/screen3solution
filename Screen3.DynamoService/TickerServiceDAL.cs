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

        public async Task InsertNewTrade(TickerCollectionEntity tickerCollection)
        {
            var doc = this.toTickerDocument(tickerCollection);

            await this.table.PutItemAsync(doc);
        }

        public async Task<List<TickerCollectionEntity>> GetAll()
        {
            List<TickerCollectionEntity> tickerCollectionList = new List<TickerCollectionEntity>();
            ScanFilter scanFilter = new ScanFilter();
            Search getAllItems = this.table.Scan(scanFilter);

            List<Document> allItems = await getAllItems.GetRemainingAsync();

            foreach (Document doc in allItems)
            {
                TickerCollectionEntity en = this.toTickerCollectionEntity(doc);
                tickerCollectionList.Add(en);
            }

            return tickerCollectionList;
        }

        public async Task Delete(string code)
        {
            DeleteItemOperationConfig config = new DeleteItemOperationConfig
            {
                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };

            await this.table.DeleteItemAsync(code, config);
        }

        public async Task<TickerCollectionEntity> GetItem(string code)
        {
            TickerCollectionEntity tickerCollection = null;

            var doc = await this.table.GetItemAsync(code);
            tickerCollection = this.toTickerCollectionEntity(doc);

            return tickerCollection;
        }

        public async Task<TickerCollectionEntity> Update(TickerCollectionEntity tickerCollection)
        {
            Document updatredAccountDoc = null;
            Document accountDoc = this.toTickerDocument(tickerCollection);

            updatredAccountDoc = await this.table.UpdateItemAsync(accountDoc);

            return this.toTickerCollectionEntity(updatredAccountDoc);
        }

        private Document toTickerDocument(TickerCollectionEntity tickerCollection)
        {
            var doc = new Document();

            doc["code"] = tickerCollection.Code;

            List<Document> tickerDocs = new List<Document>();

            if (tickerCollection.Tickers != null && tickerCollection.Tickers.Count > 0)
            {
                foreach (TickerEntity trade in tickerCollection.Tickers)
                {
                    Document tdoc = new Document();
                    tdoc["t"] = trade.T;
                    tdoc["p"] = trade.P;
                    tdoc["o"] = trade.O;
                    tdoc["h"] = trade.H;
                    tdoc["c"] = trade.C;
                    tdoc["l"] = trade.L;
                    tdoc["v"] = trade.V;

                    tickerDocs.Add(tdoc);
                }
                doc["tickers"] = tickerDocs;
            }

            return doc;
        }

        private TickerCollectionEntity toTickerCollectionEntity(Document doc)
        {
            TickerCollectionEntity en = null;

            if (doc != null)
            {
                en = new TickerCollectionEntity();
                en.Code = doc["code"].ToString();


                if (doc.ContainsKey("tickers"))
                {
                    var tradesDoc = doc["tickers"] as DynamoDBList;

                    foreach (var item in tradesDoc.Entries)
                    {
                        Document tradeDoc = item as Document;
                        TickerEntity trade = new TickerEntity();

                        trade.T = tradeDoc["t"].AsString();
                        trade.P = tradeDoc["p"].AsInt();
                        trade.O = (float)tradeDoc["o"].AsDecimal();
                        trade.H = (float)tradeDoc["h"].AsDecimal();
                        trade.L = (float)tradeDoc["l"].AsDecimal();
                        trade.C = (float)tradeDoc["c"].AsDecimal();
                        trade.V = tradeDoc["v"].AsLong();

                        en.Tickers.Add(trade);
                    }

                }
            }
            return en;
        }

    }
}

