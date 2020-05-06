using System;

namespace Screen3.Entity
{
    public class TickerEntity
    {
        public string code;
        public int period;
        public float open;
        public float high;
        public float low;
        public float close;
        public long volume;

        public override string ToString() {
            return $"{this.code},{this.period},{this.open},{this.high},{this.low},{this.close},{this.volume}";
        }
    }
}
