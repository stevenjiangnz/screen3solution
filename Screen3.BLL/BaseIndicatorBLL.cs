using System;
using Screen3.Utils;

namespace Screen3.BLL
{
    public class BaseIndicatorBLL
    { 
        public int offset = 200;

        public int? getOffsetedDate(int? period) {
            if (period.HasValue && period != 0) {
                return DateHelper.ToInt(DateHelper.ToDate(period.Value).AddDays(-1 * offset));
            } else {
                return 0;
            }
        }
        public int? getOffsetedDateWeekly(int? period) {
            if (period.HasValue && period != 0) {
                return DateHelper.ToInt(DateHelper.ToDate(period.Value).AddDays((-0.20) * offset));
            } else {
                return 0;
            }
        }
    }
}
