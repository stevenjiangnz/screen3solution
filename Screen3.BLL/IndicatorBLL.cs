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

       
        public async Task<IndSingleValueEntity[]> GetSMA(string code, int period, int? start = 0, int? end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndSingleValueEntity> outList = new List<IndSingleValueEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double?[] outSMA = new double?[len];
            
            SMA.Calculate(close, period, outSMA);

            for (int i =0; i< len; i++) {
                outList.Add(new IndSingleValueEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    V = outSMA[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }


        public async Task<IndSingleValueEntity[]> GetEMA(string code, int period, double mfactor = 2, int? start = 0, int? end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndSingleValueEntity> outList = new List<IndSingleValueEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double?[] outSMA = new double?[len];
            
            EMA.Calculate(close, period, mfactor, outSMA);

            for (int i =0; i< len; i++) {
                outList.Add(new IndSingleValueEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    V = outSMA[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndSingleValueEntity[]> GetATR(string code, int period, int? start = 0, int? end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndSingleValueEntity> outList = new List<IndSingleValueEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double[] high = tickers.Select(t => (double)t.H).ToArray(); 
            double[] low = tickers.Select(t => (double)t.L).ToArray(); 
            double?[] outATR = new double?[len];
            
            ATR.Calculate(high, low, close, period, outATR);

            for (int i =0; i< len; i++) {
                outList.Add(new IndSingleValueEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    V = outATR[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndADXEntity[]> GetADX(string code, int start = 0, int end = 0, int period = 14, string type = "day") {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndADXEntity> outList = new List<IndADXEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double[] high = tickers.Select(t => (double)t.H).ToArray(); 
            double[] low = tickers.Select(t => (double)t.L).ToArray(); 

            double?[] outADX = new double?[len];
            double?[] outDi_P = new double?[len];
            double?[] outDi_M = new double?[len];

            ADX.Calculate(high, low, close, outDi_P, outDi_M, outADX, period);


            for (int i =0; i< len; i++) {
                outList.Add(new IndADXEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    Di_plus = outDi_P[i],
                    Di_minus = outDi_M[i],
                    Adx = outADX[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndBBEntity[]> GetBB(string code, int start = 0, int end = 0, double factor = 2.0, int period = 20, string type = "day") {
            Console.WriteLine($"{code} {start} {end} {type}");
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndBBEntity> outList = new List<IndBBEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double[] high = tickers.Select(t => (double)t.H).ToArray(); 
            double[] low = tickers.Select(t => (double)t.L).ToArray(); 

            double?[] outHigh = new double?[len];
            double?[] outMid = new double?[len];
            double?[] outLow = new double?[len];

            BollingerBand.Calculate(close, period, factor, outMid, outHigh, outLow);

            for (int i =0; i< len; i++) {
                outList.Add(new IndBBEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    High = outHigh[i],
                    Mid = outMid[i],
                    Low = outLow[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndSingleValueEntity[]> GetDelt(string code, int period = 1, int start = 0, int end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndSingleValueEntity> outList = new List<IndSingleValueEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double?[] outDelt = new double?[len];
            
            Delt.Calculate(close, period, outDelt);

            for (int i =0; i< len; i++) {
                outList.Add(new IndSingleValueEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    V = outDelt[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndHeikinEntity[]> GetHeikin(string code, int start = 0, int end = 0, string type = "day") {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndHeikinEntity> outList = new List<IndHeikinEntity>();

            int len = tickers.Length;

            double[] open = tickers.Select(t => (double)t.O).ToArray(); 
            double[] close = tickers.Select(t => (double)t.C).ToArray(); 
            double[] high = tickers.Select(t => (double)t.H).ToArray(); 
            double[] low = tickers.Select(t => (double)t.L).ToArray(); 

            double?[] outOpen = new double?[len];
            double?[] outClose = new double?[len];
            double?[] outHigh = new double?[len];
            double?[] outLow = new double?[len];

            HeikinAshi.Calculate(open,close,high, low, outOpen, outClose,outHigh,outLow);

            for (int i =0; i< len; i++) {
                outList.Add(new IndHeikinEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    Open = outOpen[i],
                    Close = outClose[i],
                    High = outHigh[i],
                    Low = outLow[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }

        public async Task<IndMACDEntity[]> GetMACD(string code, int slow = 26, int fast = 12, int signal = 9, int start = 0, int end = 0, string type = "day" ) {
            TickerEntity[] tickers = await base.getTickerEntityArray(code, start, end, type);
            List<IndMACDEntity> outList = new List<IndMACDEntity>();

            int len = tickers.Length;

            double[] close = tickers.Select(t => (double)t.C).ToArray(); 

            double?[] outMACD = new double?[len];
            double?[] outSignal = new double?[len];
            double?[] outHist = new double?[len];
            
            MACD.Calculate(close, slow, fast, signal, outMACD, outSignal, outHist);

            for (int i =0; i< len; i++) {
                outList.Add(new IndMACDEntity{
                    T = tickers[i].T,
                    P = tickers[i].P,
                    MACD = outMACD[i],
                    Signal = outSignal[i],
                    Hist = outHist[i]
                });
            }
            
            return outList.Where(r => (start == 0 || r.P >= start) && (end ==0 || r.P <= end)).ToArray();
        }


    }
}