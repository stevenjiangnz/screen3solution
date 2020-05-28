using System;
using Screen3.Utils;
namespace Screen3.Entity
{
    public class IndStochasticEntity
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
        public double? K { get; set; }
        public double? D { get; set; }

#nullable disable
    }
}