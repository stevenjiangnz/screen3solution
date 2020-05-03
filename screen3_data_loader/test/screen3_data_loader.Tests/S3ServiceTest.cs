using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using screen3_data_loader.services;

namespace screen3_data_loader.services.Tests
{
    public class S3ServiceTest
    {
        [Fact]
        public void TestToConnect()
        {
            var client = new S3Service();
            client.Connect().Wait();
        }
    }
}
