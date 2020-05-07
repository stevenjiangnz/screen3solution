using System;
using System.Collections.Generic;
using Screen3.Entity;

namespace Screen3.BLL
{
    public class TickerComparer<TickerEntity> : IEqualityComparer<TickerEntity>
    {
        public bool Equals(TickerEntity t1, TickerEntity t2)
        {
            if (t1.ToString().ToUpper() == t2.ToString().ToUpper())
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(TickerEntity obj)
        {
            return 0;
        }
    }

}