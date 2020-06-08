using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using Screen3.Utils;

namespace Screen3.BLL
{
    public class Screen_MACD_William : IScreenInterface
    {
        private string INDEX_CODE = "XAO";
        private double WILLIAM_BUY_LEVEL = -80;
        private double WILLIAM_SELL_LEVEL = -20;

        private double MACD_BUY_LEVEL = 0;
        private double MACD_SELL_LEVEL = 0;

        private int DECLUSTER = 2;  // greate than 0, mean will decluster, 
        private string DIRECTION = "BUY";

        private TickerEntity[] priceTickerList;
        private TickerEntity[] indexTickerList;
        private IndMACDEntity[] macdList;
        private IndSingleValueEntity[] williamList;

        private TickerBLL tickerBLL;
        private IndicatorBLL indicatorBLL;

        public Screen_MACD_William(string bucketName, string localFolder)
        {
            this.tickerBLL = new TickerBLL(bucketName, localFolder);
            this.indicatorBLL = new IndicatorBLL(bucketName, localFolder);
        }

        public List<ScreenResultEntity> GetEntryMatchTickers(IDictionary<string, object> options)
        {
            if (options != null)
            {
                if (options.Keys.Contains("WILLIAM_BUY_LEVEL"))
                {
                    this.WILLIAM_BUY_LEVEL = double.Parse(options["WILLIAM_BUY_LEVEL"].ToString());
                }

                if (options.Keys.Contains("WILLIAM_SELL_LEVEL"))
                {
                    this.WILLIAM_SELL_LEVEL = double.Parse(options["WILLIAM_SELL_LEVEL"].ToString());
                }

                if (options.Keys.Contains("MACD_BUY_LEVEL"))
                {
                    this.MACD_BUY_LEVEL = double.Parse(options["MACD_BUY_LEVEL"].ToString());
                }

                if (options.Keys.Contains("MACD_SELL_LEVEL"))
                {
                    this.MACD_SELL_LEVEL = double.Parse(options["MACD_SELL_LEVEL"].ToString());
                }

                if (options.Keys.Contains("DECLUSTER"))
                {
                    this.DECLUSTER = int.Parse(options["DECLUSTER"].ToString());
                }

                if (options.Keys.Contains("DIRECTION"))
                {
                    this.DIRECTION = options["DIRECTION"].ToString().ToUpper();
                }
            }

            List<TickerEntity> matchedList = new List<TickerEntity>();
            List<ScreenResultEntity> resultList = new List<ScreenResultEntity>();

            int len = this.priceTickerList.Length;

            for (int i = 0; i < len; i++)
            {
                if (this.macdList[i].MACD != null && this.williamList[i].V.HasValue)
                {
                    if (this.DIRECTION.ToUpper().IndexOf("BUY") >= 0)
                    {
                        if ((this.macdList[i].MACD >= this.MACD_BUY_LEVEL || this.macdList[i].Signal >= this.MACD_BUY_LEVEL) &&
                        this.williamList[i].V <= this.WILLIAM_BUY_LEVEL)
                        {

                            if (matchedList.Count > 0)
                            {
                                if ((this.priceTickerList[i].P - matchedList[matchedList.Count - 1].P) > this.DECLUSTER)
                                {
                                    matchedList.Add(this.priceTickerList[i]);
                                    resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "BUY" });
                                }
                            }
                            else
                            {
                                matchedList.Add(this.priceTickerList[i]);
                                resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "BUY" });
                            }
                        }
                    }

                    if (this.DIRECTION.ToUpper().IndexOf("SELL") >= 0)
                    {
                        {
                            if ((this.macdList[i].MACD <= this.MACD_SELL_LEVEL || this.macdList[i].Signal <= this.MACD_SELL_LEVEL) &&
                            this.williamList[i].V >= this.WILLIAM_SELL_LEVEL)
                            {

                                if (matchedList.Count > 0)
                                {
                                    if ((this.priceTickerList[i].P - matchedList[matchedList.Count - 1].P) > this.DECLUSTER)
                                    {
                                        matchedList.Add(this.priceTickerList[i]);
                                        resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "SELL" });
                                    }
                                }
                                else
                                {
                                    matchedList.Add(this.priceTickerList[i]);
                                    resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "SELL" });
                                }
                            }
                        }
                    }
                }
            }
            return resultList;
        }

        public async Task<List<ScreenResultEntity>> DoScreen(string code, string type = "day", int start = 0, int end = 0, IDictionary<string, object> options = null)
        {
            if (type == "day")
            {
                this.priceTickerList = (await this.tickerBLL.GetDailyTickerEntityList(code, start, end)).ToArray();
                this.indexTickerList = (await this.tickerBLL.GetDailyTickerEntityList(INDEX_CODE, start, end)).ToArray();
            }
            this.macdList = await this.indicatorBLL.GetMACD(code: code, start: start, end: end, type: type);
            this.williamList = await this.indicatorBLL.GetWilliamR(code: code, start: start, end: end, type: type);

            List<ScreenResultEntity> resultList = this.GetEntryMatchTickers(options);

            return resultList;
        }
    }
}
