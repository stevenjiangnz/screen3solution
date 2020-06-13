using System;
using System.Collections.Generic;

namespace Screen3.Entity
{
    public class AccountEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TradeEntity> Trades { get; set; }
    }
}