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
    }
}
