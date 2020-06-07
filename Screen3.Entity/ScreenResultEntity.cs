using System;
using Screen3.Utils;
namespace Screen3.Entity
{
    public class ScreenResultEntity
    {
        public String Code { get; set; } // Code, Ticker
        public int P { get; set; } // Period
        public long P_Stamp
        {
            get
            {
                return DateHelper.ToTimeStamp(P);
            }
        }
    }
}