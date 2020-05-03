using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using screen3_data_loader.services;

namespace screen3_data_loader.controllers
{
    public class DataLoadController
    {
        private string S3_Bucket_Name;
        public async Task<String> StartProcessAsync()
        {
            string result = "";

            this.Init();

            List<S3Object> fileList = await this.GetSourceFileListAsync(this.S3_Bucket_Name, "source");

            LambdaLogger.Log("About to start data loading process..." + S3_Bucket_Name);

            return result;
        }

        public async Task<List<S3Object>> GetSourceFileListAsync(string bucketName, string path) {
            S3Service service = new S3Service();
            List<S3Object> fileList = await service.ListingObjectsAsync(bucketName, path);

            Console.WriteLine("return item count: " + fileList.Count);
            return fileList;
        }

        public void Init()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
        }
    }
}