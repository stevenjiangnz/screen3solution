using System;

namespace Screen3.Entity
{
    public class IndSmaEntity
    {
        public String C {get;set;} // Code
        public int P {get;set;} // Period

        #nullable enable
        public double? V {get;set;} // Value
        #nullable disable
    }
}
