using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Screen3.S3Service;
using Screen3.Entity;
using CsvHelper;

namespace screen3_data_loader.controllers
{
    public class StockLoadController
    {
        private string S3_Bucket_Name;
        private string Temp_Folder;
        private S3Service s3service;

        public StockLoadController()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
            this.s3service = new S3Service();
        }

        public async Task LoadAsx300Async()
        {
            string resultPath = await this.s3service.DownloadFileFromS3Async(this.S3_Bucket_Name, "asx300.csv", this.Temp_Folder + "asx300/");

            var stockList = this.LoadStockFromCSV(resultPath);

            string json = JsonConvert.SerializeObject(stockList, Formatting.Indented);
            Console.WriteLine(json);
        }

        public List<StockEntity> LoadStockFromCSV(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<StockEntity>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new StockEntity();

                    record.Code = csv.GetField<string>("code");
                    record.Company = csv.GetField<string>("company");
                    record.Sector = csv.GetField<string>("sector");
                    record.Cap = csv.GetField<double>("cap");
                    record.Weight = csv.GetField<double>("weight");

                    records.Add(record);
                }

                return records;
            }
        }
    }
}