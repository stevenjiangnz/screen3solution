using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Screen3.S3Service;
using screen3_data_loader.utils;

namespace screen3_data_loader.controllers
{
    public class StockLoadController
    {
        private string S3_Bucket_Name;
        private string Temp_Folder;
        private S3Service s3service;

        public StockLoadController() {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
            this.s3service = new S3Service();
        }

        public async Task LoadAsx300Async()
        {
            string result = await this.s3service.DownloadFileFromS3Async(this.S3_Bucket_Name, "asx300.csv", this.Temp_Folder + "asx300/");

            Console.WriteLine("download result: " + result);
        }

        public void LoadStockFromCSV (string path) {
            

        }

    }
}