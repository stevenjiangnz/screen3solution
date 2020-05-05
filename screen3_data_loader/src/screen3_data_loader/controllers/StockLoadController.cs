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
using Screen3.DynamoService;

using CsvHelper;

namespace screen3_data_loader.controllers
{
    public class StockLoadController
    {
        private string S3_Bucket_Name;
        private string Temp_Folder;
        private S3Service s3service;
        private StockServiceDAL dal;
        public StockLoadController()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
            this.s3service = new S3Service();
            string asx300TableName = Environment.GetEnvironmentVariable("SCREEN3_ASX300_TABLE_NAME");
            this.dal = new StockServiceDAL(asx300TableName);
        }

        public async Task LoadAsx300Async()
        {
            LambdaLogger.Log($"In Load Asx300Async...\n");
            var stockList = await dal.GetAll();

            LambdaLogger.Log($"Found existing items {stockList.Count}, abou to remove them all.\n");

            foreach(StockEntity stock in stockList) {
                await dal.Delete(stock.Code);
            }

            LambdaLogger.Log($"Remove all items done.\n");

            LambdaLogger.Log($"About populate all stocks.\n");

            string resultPath = await this.s3service.DownloadFileFromS3Async(this.S3_Bucket_Name, "asx300.csv", this.Temp_Folder + "asx300/");

            var newStockList = this.LoadStockFromCSV(resultPath);

            foreach(var s in newStockList) {
                await this.dal.InsertNewStock(s);
            }

            LambdaLogger.Log($"{newStockList.Count} new items been populated into the db.\n");
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