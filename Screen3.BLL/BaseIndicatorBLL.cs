using System;
using System.Collections.Generic;
using Screen3.Utils;
using Screen3.Entity;
using System.Threading.Tasks;

namespace Screen3.BLL
{
    public class BaseIndicatorBLL
    { 
        public int offset = 200;
        protected TickerBLL tickerBLL;

        public int getOffsetedDate(int? period) {
            if (period.HasValue && period != 0) {
                return DateHelper.ToInt(DateHelper.ToDate(period.Value).AddDays(-1 * offset));
            } else {
                return 0;
            }
        }

        public async Task<TickerEntity[]> getTickerEntityArray(string code, int? start, int? end, string type = "day") {
            int offsetedStarted;
            TickerEntity[] tickers;
            
            offsetedStarted = this.getOffsetedDate(start);

            if (type == "week") {
                tickers = (await this.tickerBLL.GetWeeklyTickerEntityList(code.ToUpper(), offsetedStarted, end)).ToArray();
            } else {
                tickers = (await this.tickerBLL.GetDailyTickerEntityList(code.ToUpper(), offsetedStarted, end)).ToArray();
            }

            return tickers;
        }
        
    }
}
