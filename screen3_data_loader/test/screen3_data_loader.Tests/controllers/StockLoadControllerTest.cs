using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using screen3_data_loader.controllers;

namespace screen3_data_loader.controllers.Tests
{
    public class StockLoadControllerTest
    {
        [Fact]
        public void TestLoadAsx300Async()
        {
            StockLoadController controller = new StockLoadController();

            controller.LoadAsx300Async().Wait();
        }

        [Fact]
        public void TestLoadStockFromCSV()
        {
            StockLoadController controller = new StockLoadController();

            controller.LoadStockFromCSV(@"/tmp/screen3_temp_files/asx300/asx300.csv");
        }
    }
}
