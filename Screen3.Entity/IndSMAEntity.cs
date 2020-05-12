using System;

namespace Screen3.Entity
{
    public class IndSMAEntity
    {
        public String T {get;set;} // Code, Ticker
        public int P {get;set;} // Period

        #nullable enable
        public double? V {get;set;} // Value
        #nullable disable
    }
}
