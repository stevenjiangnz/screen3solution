using System;
using System.Threading.Tasks;
using Screen3.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;

using Screen3.Utils;

namespace Screen3.DynamoService
{
    public class TradeServiceDAL
    {
        private AmazonDynamoDBClient client;
        private Table table;

        public TradeServiceDAL(string tableName)
        {
            this.client = new AmazonDynamoDBClient();
            this.table = Table.LoadTable(this.client, tableName);
        }

        public async Task InsertNewTrade(AccountEntity account)
        {
            var doc = this.toAccountDocument(account);

            await this.table.PutItemAsync(doc);
        }

        public async Task<List<AccountEntity>> GetAll()
        {
            List<AccountEntity> accountList = new List<AccountEntity>();
            ScanFilter scanFilter = new ScanFilter();
            Search getAllItems = this.table.Scan(scanFilter);

            List<Document> allItems = await getAllItems.GetRemainingAsync();

            foreach (Document doc in allItems)
            {
                AccountEntity en = this.toAccountEntity(doc, false);
                accountList.Add(en);
            }

            return accountList;
        }

        public async Task Delete(string id)
        {
            List<StockEntity> stockList = new List<StockEntity>();
            DeleteItemOperationConfig config = new DeleteItemOperationConfig
            {
                // Return the deleted item.
                ReturnValues = ReturnValues.AllOldAttributes
            };

            await this.table.DeleteItemAsync(id, config);
        }

        public async Task<AccountEntity> GetItem(string id)
        {
            AccountEntity account = null;

            var doc = await this.table.GetItemAsync(id);
            account = this.toAccountEntity(doc, true);

            return account;
        }

        public async Task<AccountEntity> Update(AccountEntity account)
        {
            Document updatredAccountDoc = null;
            Document accountDoc = this.toAccountDocument(account);

            updatredAccountDoc = await this.table.UpdateItemAsync(accountDoc);

            return this.toAccountEntity(updatredAccountDoc);
        }


        private Document toAccountDocument(AccountEntity account)
        {
            var doc = new Document();

            doc["id"] = account.Id;
            doc["name"] = account.Name;

            List<Document> trades = new List<Document>();

            if (account.Trades != null && account.Trades.Count > 0)
            {
                foreach (TradeEntity tr in account.Trades)
                {
                    trades.Add(this.toTradeDocument(tr));
                }

                doc["trades"] = trades;

            }

            return doc;
        }

        private AccountEntity toAccountEntity(Document doc, bool includeTrades = false)
        {
            AccountEntity en = null;

            if (doc != null)
            {
                en = new AccountEntity();
                en.Id = doc["id"].ToString();
                en.Name = doc.ContainsKey("name") ? doc["name"].ToString() : "";

                if (includeTrades)
                {
                    en.Trades = new List<TradeEntity>();

                    if (doc.ContainsKey("trades"))
                    {
                        var tradesDoc = doc["trades"] as DynamoDBList;

                        foreach (var item in tradesDoc.Entries)
                        {
                            Document tradeDoc = item as Document;
                            TradeEntity trade = new TradeEntity();

                            trade.Id = tradeDoc["id"].AsString();
                            trade.Code = tradeDoc["code"].AsString();
                            trade.Direction = tradeDoc["direction"].AsInt();
                            trade.EntryDate = tradeDoc["entryDate"].AsInt();
                            trade.EntryPrice = tradeDoc["entryPrice"].AsDouble();

                            if (tradeDoc.ContainsKey("exitDate"))
                            {
                                trade.ExitDate = tradeDoc["exitDate"].AsInt();
                            }

                            if (tradeDoc.ContainsKey("exitPrice"))
                            {
                                trade.ExitPrice = tradeDoc["exitPrice"].AsDouble();
                            }

                            if (tradeDoc.ContainsKey("pl"))
                            {
                                trade.PL = tradeDoc["pl"].AsDouble();
                            }
                            en.Trades.Add(trade);
                        }

                    }
                }
            }
            return en;
        }

        private Document toTradeDocument(TradeEntity trade)
        {
            Document doc = new Document();

            doc["id"] = trade.Id;
            doc["code"] = trade.Code;
            doc["direction"] = trade.Direction;
            doc["entryDate"] = trade.EntryDate;
            doc["entryPrice"] = trade.EntryPrice;
            doc["exitDate"] = trade.ExitDate;
            doc["exitPrice"] = trade.ExitPrice;
            doc["pl"] = trade.PL;

            return doc;
        }
    }
}

