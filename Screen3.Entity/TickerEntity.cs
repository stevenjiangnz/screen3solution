using System;

namespace Screen3.Entity
{
    public class TickerEntity
    {
        public string Code;
        public int Period;
        public float Open;
        public float High;
        public float Low;
        public float Close;
        public long Volume;

        public override string ToString() {
            return $"{this.Code},{this.Period},{this.Open},{this.High},{this.Low},{this.Close},{this.Volume}";
        }
    }
}
