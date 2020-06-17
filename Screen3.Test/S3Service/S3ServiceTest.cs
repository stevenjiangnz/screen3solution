using System;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.S3Service;

namespace Screen3.Test.DynamoService
{
    public class S3ServiceTest
    {
        [Fact]
        public void TestUploadFileToS3Async()
        {
            S3Service.S3Service service = new S3Service.S3Service();

            const string bucket = "stevenjiangnz-screen3-eod-source";
            string key="source/test.zip";
            string path = "/home/steven/devlocal/screen3solution/Screen3.Test/obj/project.assets.json";

            service.UploadFileToS3Async(bucket, key, path).Wait();
        }

    }
}
