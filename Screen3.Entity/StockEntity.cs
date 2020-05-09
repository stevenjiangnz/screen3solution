using System;

namespace Screen3.Entity
{
    public class StockEntity
    {
        public String Code {get;set;}
        public String Company {get;set;}

        #nullable enable
        public String? Sector {get;set;}
        public Double? Cap {get;set;}
        public Double? Weight {get;set;}
        #nullable disable
    }
}
