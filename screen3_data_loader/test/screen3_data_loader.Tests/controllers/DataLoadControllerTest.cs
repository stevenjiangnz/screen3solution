using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using screen3_data_loader.controllers;
using Screen3.Utils;

namespace screen3_data_loader.controllers.Tests
{
    public class DataLoadControllerTest
    {
        [Fact]
        public void TestStartProcessAsync()
        {
            DataLoadController controller = new DataLoadController();

            controller.StartProcessAsync().Wait();
        }


        [Fact]
        public void TestGetSourceFileListAsync() {
            DataLoadController controller = new DataLoadController();

            controller.GetSourceFileListAsync("stevenjiangnz-screen3-eod-source", "source").Wait();
        }


        [Fact]
        public void TestExtractIntoDayData() {
            DataLoadController controller = new DataLoadController();

            controller.ExtractIntoDayData("/tmp/screen3_temp_files/originSourceFiles/2007-2012.zip", "/tmp/screen3_temp_files/originExtractedFiles/");
        }


        [Fact]
        public void TestLoadTickerFromCSV()
        {
            DataLoadController controller = new DataLoadController();
            string path = @"/tmp/screen3_temp_files/originExtractedFiles/source/1997-2006.zip/1997-2006/20061229.TXT";

            string tickerString = ObjectHelper.ToJson(controller.LoadTickerFromCSV(path));

            Console.WriteLine(tickerString);
        }
    }
}
