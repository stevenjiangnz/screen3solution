using System;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
namespace Screen3.Test.BLL
{
    public class TickerBLLTest
    {
        [Fact]
        public void TestSaveTickers() {
            TickerBLL bll = new TickerBLL();

            bll.SaveTickers("CLL", null);
        }
    }
}
