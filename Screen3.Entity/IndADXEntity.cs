using System;
using Screen3.Utils;

namespace Screen3.Entity
{
    public class IndADXEntity
    {
        public String T { get; set; } // Code, Ticker
        public int P { get; set; } // Period
        public long P_Stamp
        {
            get
            {
                return DateHelper.ToTimeStamp(P);
            }
        }
#nullable enable
        public double? Di_plus { get; set; }
        public double? Di_minus { get; set; }
        public double? Adx { get; set; }

#nullable disable
    }
}
