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

        [Fact]
        public void TestListObjectsAsync()
        {
            var client = new S3Service();
            String bucketName = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");

            client.ListingObjectsAsync(bucketName, "source").Wait();
        }



    }
}
