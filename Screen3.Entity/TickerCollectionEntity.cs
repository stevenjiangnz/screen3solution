using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Screen3.Utils;

namespace Screen3.Entity
{
    public class TickerCollectionEntity
    {
        public TickerCollectionEntity()
        {
            this.Tickers = new List<TickerEntity>();
        }

        public string Code { get; set; }
        public List<TickerEntity> Tickers
        {
            get; set;
        }


    }
}
