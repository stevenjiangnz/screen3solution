using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using Screen3.Utils;

namespace Screen3.BLL
{
    public class Screen_ADX : IScreenInterface
    {
        private string INDEX_CODE = "XAO";
        private double OFFSET = 0;
        private string DIRECTION = "BUY";

        private TickerEntity[] priceTickerList;
        private TickerEntity[] indexTickerList;
        private IndADXEntity[] adxList;

        private TickerBLL tickerBLL;
        private IndicatorBLL indicatorBLL;

        public Screen_ADX(string bucketName, string localFolder)
        {
            this.tickerBLL = new TickerBLL(bucketName, localFolder);
            this.indicatorBLL = new IndicatorBLL(bucketName, localFolder);
        }

        public List<ScreenResultEntity> GetEntryMatchTickers(IDictionary<string, object> options)
        {
            if (options != null)
            {
                if (options.Keys.Contains("OFFSET"))
                {
                    this.OFFSET = double.Parse(options["OFFSET"].ToString());
                }

                if (options.Keys.Contains("DIRECTION"))
                {
                    this.DIRECTION = options["DIRECTION"].ToString().ToUpper();
                }
            }

            List<TickerEntity> matchedList = new List<TickerEntity>();
            List<ScreenResultEntity> resultList = new List<ScreenResultEntity>();

            int len = this.priceTickerList.Length;

            for (int i = 1; i < len; i++)
            {
                if (this.adxList[i].Adx != null && this.adxList[i].Adx.HasValue && this.adxList[i].Di_plus.HasValue && this.adxList[i].Di_minus.HasValue &&
                this.adxList[i - 1].Adx != null && this.adxList[i - 1].Adx.HasValue && this.adxList[i - 1].Di_plus.HasValue && this.adxList[i - 1].Di_minus.HasValue)
                {
                    if (this.DIRECTION.ToUpper().IndexOf("BUY") >= 0)
                    {
                        if (this.adxList[i].Di_plus.Value > this.adxList[i].Di_minus.Value &&
                        this.adxList[i].Di_minus.Value < this.adxList[i - 1].Di_minus.Value &&
                        this.adxList[i].Adx.Value > this.adxList[i - 1].Adx.Value &&
                        this.adxList[i - 1].Adx.Value < this.adxList[i - 1].Di_plus.Value &&
                        this.adxList[i - 1].Adx.Value < this.adxList[i - 1].Di_minus.Value &&
                        (this.adxList[i].Adx.Value + this.OFFSET) >= this.adxList[i - 1].Di_minus.Value)
                        {
                            resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "BUY" });
                        }
                    }

                    if (this.DIRECTION.ToUpper().IndexOf("SELL") >= 0)
                    {
                        if (this.adxList[i].Di_plus.Value < this.adxList[i].Di_minus.Value &&
                        this.adxList[i].Di_plus.Value < this.adxList[i - 1].Di_plus.Value &&
                        this.adxList[i].Adx.Value > this.adxList[i - 1].Adx.Value &&
                        this.adxList[i - 1].Adx.Value < this.adxList[i - 1].Di_plus.Value &&
                        this.adxList[i - 1].Adx.Value < this.adxList[i - 1].Di_minus.Value &&
                        (this.adxList[i].Adx.Value + this.OFFSET) >= this.adxList[i - 1].Di_plus.Value)
                        {
                            resultList.Add(new ScreenResultEntity { Code = this.priceTickerList[i].T, P = this.priceTickerList[i].P, Direction = "SELL" });
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
            this.adxList = await this.indicatorBLL.GetADX(code: code, start: start, end: end, type: type);

            List<ScreenResultEntity> resultList = this.GetEntryMatchTickers(options);

            return resultList;
        }
    }
}
