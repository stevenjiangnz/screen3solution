using System;
using System.IO;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Test.BLL
{
    public class StockBLLTest
    {
        private string tableName = "stevenjiangnz-screen3-asx300";

        [Fact]
        public async void TestGetAll_ASX300()
        {
            StockBLL bll = new StockBLL(tableName);
            var result = await bll.GetAll();

            Console.WriteLine(ObjectHelper.ToJson(result));
        }

    }
}