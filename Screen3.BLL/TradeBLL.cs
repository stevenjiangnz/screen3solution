using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using System.Text;
using Screen3.Utils;
using Screen3.DynamoService;

namespace Screen3.BLL
{
    public class TradeBLL
    {
        private TradeServiceDAL dal;
        public TradeBLL(string tableName)
        {
            this.dal = new TradeServiceDAL(tableName);
        }

        public Task<List<AccountEntity>> GetAll()
        {
            return this.dal.GetAll();
        }

        public Task NewAccount(string id, string name)
        {
            AccountEntity account = new AccountEntity { Id = id, Name = name };

            return this.dal.InsertNewTrade(account);
        }

        public Task DeleteAccount(string id)
        {
            return this.dal.Delete(id);
        }

        public Task<AccountEntity> GetAccountDetail(string id)
        {
            return this.dal.GetItem(id);
        }

        public async Task OpenPosition(string accountId, string tradeId, string code, int direction, double entryPrice, int entryDate)
        {
            AccountEntity account = await this.dal.GetItem(accountId);

            if (account.Trades == null)
            {
                account.Trades = new List<TradeEntity>();
            }

            TradeEntity trade = new TradeEntity
            {
                Id = tradeId,
                Code = code,
                Direction = direction,
                EntryDate = entryDate,
                EntryPrice = entryPrice
            };

            account.Trades.Add(trade);

            await this.dal.Update(account);
        }

        public async Task ClosePosition(string accountId, string tradeId, double exitPrice, int exitDate)
        {
            AccountEntity account = await this.dal.GetItem(accountId);

            if (account.Trades == null)
            {
                account.Trades = new List<TradeEntity>();
            }

            foreach (TradeEntity trade in account.Trades)
            {
                if (trade.Id == tradeId)
                {
                    trade.ExitPrice = exitPrice;
                    trade.ExitDate = exitDate;
                    if (trade.EntryPrice != 0)
                    {
                        trade.PL = (((trade.ExitPrice - trade.EntryPrice) / trade.EntryPrice) * 100 * trade.Direction);
                        trade.PL = Math.Round(trade.PL.Value * 100) / 100;
                    }
                }
            }
            await this.dal.Update(account);
        }


    }
}