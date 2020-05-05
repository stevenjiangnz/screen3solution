using System;

namespace Screen3.Entity
{
    public class StockEntity
    {
        public String Code;
        public String Company;

        #nullable enable
        public String? Sector;
        public Double? Cap;
        public Double? Weight;
        #nullable disable
    }
}
