using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using screen3_data_loader;

namespace screen3_data_loader.Tests
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
