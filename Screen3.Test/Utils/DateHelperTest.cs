using System;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Test.Utils
{
    public class DateHelperTest
    {
        [Fact]
        public void TestToDate(){
            int period = 19980401;
            var dt = DateHelper.ToDate(period);
            int dtInt = DateHelper.ToInt(dt);

            Console.WriteLine("dt: " + dt.ToLongDateString()) ;
            Console.WriteLine("dtint: " + dtInt);

            Assert.Equal(period, dtInt);
        }

        [Fact]
        public void TestEndOfWeek() {
            int period = 19980401;

            Console.WriteLine("friday: " + DateHelper.EndOfWeek(period) +  "   " + DateHelper.ToDate(period).ToLongDateString());
        }
    }
}