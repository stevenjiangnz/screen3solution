using System;

namespace Screen3.Entity
{
    public class TickerEntity
    {
        public TickerEntity()
        {

        }

        public TickerEntity(string tickerString)
        {
            if (!string.IsNullOrEmpty(tickerString))
            {
                string[] tickerParts = tickerString.Split(',');

                for (int i = 0; i < tickerParts.Length; i++)
                {
                        this.Code = tickerParts[0];
                        this.Period = int.Parse(tickerParts[1]);
                        this.Open = float.Parse(tickerParts[2]);
                        this.High = float.Parse(tickerParts[3]);
                        this.Low = float.Parse(tickerParts[4]);
                        this.Close = float.Parse(tickerParts[5]);
                        this.Volume = long.Parse(tickerParts[6]);

                }
            }
        }

        public string Code;
        public int Period;
        public float Open;
        public float High;
        public float Low;
        public float Close;
        public long Volume;

        public override string ToString()
        {
            return $"{this.Code},{this.Period},{this.Open},{this.High},{this.Low},{this.Close},{this.Volume}";
        }

    }
}
