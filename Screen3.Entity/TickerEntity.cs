using System;
using System.Runtime.Serialization;

namespace Screen3.Entity
{
    [DataContract]
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

                if (tickerParts.Length > 6)
                {
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
        }
        [DataMember]
        public string Code {get;set;}
        [DataMember]
        public int Period {get;set;}
        [DataMember]
        public float Open {get;set;}
        [DataMember]
        public float High {get;set;}
        [DataMember]
        public float Low {get;set;}
        [DataMember]
        public float Close {get;set;}
        [DataMember]
        public long Volume {get;set;}

        public override string ToString()
        {
            return $"{this.Code},{this.Period},{this.Open},{this.High},{this.Low},{this.Close},{this.Volume}";
        }

    }
}
