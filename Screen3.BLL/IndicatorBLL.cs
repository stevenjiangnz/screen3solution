using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using System.Text;
using Screen3.Utils;
using Screen3.Indicator;


namespace Screen3.BLL
{
    public class IndicatorBLL : BaseIndicatorBLL
    { 
        private string S3_Bucket_Name;
        private string localFolder;
        public IndicatorBLL(string bucketName, string localFolder) {
            this.S3_Bucket_Name = bucketName;
            this.localFolder = localFolder;
            this.tickerBLL = new TickerBLL(this.S3_Bucket_Name, this.localFolder);
        }

        public async Task<IndSmaEntity[]> GetSMA(string code, int period, int? start = 0, int? end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndSmaEntity> outList = new List<IndSmaEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double?[] outSMA = new double?[len];
            
            SMA.Calculate(close, period, outSMA);

            for (int i =0; i< len; i++) {
                outList.Add(new IndSmaEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    V = outSMA[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }
    }
}