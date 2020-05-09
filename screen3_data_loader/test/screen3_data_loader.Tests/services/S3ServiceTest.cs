using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Screen3.S3Service;

namespace screen3_data_loader.services.Tests
{
    public class S3ServiceTest
    {
        [Fact]
        public void TestListObjectsAsync()
        {
            var client = new S3Service();
            String bucketName = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");

            client.ListingObjectsAsync(bucketName, "source").Wait();
        }

        [Fact]
        public void TestDownloadFileFromS3Async()
        {
            var client = new S3Service();
            String bucketName = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            string targetFolder = "/tmp/screen3_temp_files/originSourceFiles/";

            client.DownloadFileFromS3Async(bucketName, "source/1997-2006.zip", targetFolder).Wait();
        }

        [Fact]
        public void TestCopyObjectAsync()
        {
            var client = new S3Service();
            String bucketName = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");

            client.CopyObject(bucketName, @"source/1997-2006.zip", bucketName, @"archive/1997-2006.zip").Wait();
        }


        [Fact]
        public void TestDeleteObjectAsync()
        {
            var client = new S3Service();
            String bucketName = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");

            client.DeleteObject(bucketName, @"source/1997-2006.zip").Wait();
        }
    }
}
