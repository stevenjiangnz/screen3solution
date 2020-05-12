using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using System.Globalization;
using Amazon.S3;
using Amazon.S3.Model;
using Screen3.S3Service;
using Screen3.Entity;
using Screen3.Utils;
using Screen3.DynamoService;
using Screen3.BLL;
using CsvHelper;


namespace screen3_data_loader.controllers
{
    public class DataLoadController
    {
        private string S3_Bucket_Name;
        private string Temp_Folder;
        private List<StockEntity> stockList;
        private StockServiceDAL dal;
        private Dictionary<String, List<TickerEntity>> stockDict;

        public DataLoadController()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
            string asx300TableName = Environment.GetEnvironmentVariable("SCREEN3_ASX300_TABLE_NAME");
            this.dal = new StockServiceDAL(asx300TableName);
            this.stockDict = new Dictionary<string, List<TickerEntity>>();
        }

        public async Task StartProcessAsync()
        {
            this.stockList = await this.dal.GetAll();

            // retrieve all stock list.
            this.stockList = await this.dal.GetAll();

            List<S3Object> fileList = await this.GetSourceFileListAsync(this.S3_Bucket_Name, "source");

            LambdaLogger.Log($"found files in the source folder, {fileList.Count}\n");

            foreach (var fileInfo in fileList)
            {
                if (fileInfo.Size > 0)
                {
                    await this.ProcessSourceFile(fileInfo);
                }
            }

        }

        public async Task ProcessSourceFile(S3Object fileInfo)
        {
            LambdaLogger.Log($"About to download file, {fileInfo.Key}\n");
            String resultFileName = await this.DownloadFileAsync(fileInfo.BucketName, fileInfo.Key, this.Temp_Folder + "originSourceFiles/");

            string dailyTargetFolder = this.Temp_Folder + $"originExtractedFiles/{fileInfo.Key}/";
            LambdaLogger.Log($"Download file, {fileInfo.Key}, about to extract to {dailyTargetFolder}.\n");
            List<String> dailyFileList = this.ExtractIntoDayData(resultFileName, dailyTargetFolder);
            LambdaLogger.Log($"Extracted files done. items {dailyFileList.Count} \n");

            foreach (string path in dailyFileList)
            {
                this.AddDailyFileIntoStockDict(path);
            }

            TickerBLL bll = new TickerBLL(this.S3_Bucket_Name, Temp_Folder);
            // save tickers into S3
            foreach (KeyValuePair<string, List<TickerEntity>> tickerGroup in this.stockDict)
            {
                await bll.SaveTickersToS3(tickerGroup.Key, tickerGroup.Value, true);

                LambdaLogger.Log($"Save to S3 for {tickerGroup.Key} with items {tickerGroup.Value.Count} \n");
            }

            // move file to archive
            S3Service service = new S3Service();
            string archiveKey = fileInfo.Key.Replace("source", "archive");
            await service.CopyObject(this.S3_Bucket_Name, fileInfo.Key, this.S3_Bucket_Name, archiveKey);
            await service.DeleteObject(this.S3_Bucket_Name, fileInfo.Key);

            // about to clean things
            File.Delete(resultFileName);
            FileHelper.ClearDirectory(this.Temp_Folder + $"originExtractedFiles/", true);

            this.stockDict = new Dictionary<string, List<TickerEntity>>();

            LambdaLogger.Log("Temp folder cleared");
        }

        public List<String> ExtractIntoDayData(string fileName, string targetPath)
        {
            List<String> resultFileList = new List<String>();

            FileHelper.ClearDirectory(targetPath, true);

            ZipFile.ExtractToDirectory(fileName, targetPath);

            resultFileList = FileHelper.DirSearch(targetPath);

            return resultFileList;

        }

        public async Task<List<S3Object>> GetSourceFileListAsync(string bucketName, string path)
        {
            S3Service service = new S3Service();
            List<S3Object> fileList = await service.ListingObjectsAsync(bucketName, path);

            return fileList;
        }

        public async Task<String> DownloadFileAsync(string bucketName, string keyName, string tempFolder)
        {
            LambdaLogger.Log($"In DownloadFileAsync. bucketName: {bucketName}, keyName: {keyName}, tempFolder: {tempFolder}.\n");
            S3Service service = new S3Service();

            String resultFileName = await service.DownloadFileFromS3Async(bucketName, keyName, tempFolder);

            return resultFileName;
        }

        public async void AddDailyFileIntoStockDict(string path)
        {
            if (this.stockList == null || this.stockList.Count == 0)
            {
                this.stockList = await this.dal.GetAll();
            }

            List<TickerEntity> tickerList = this.LoadTickerFromCSV(path);

            foreach (TickerEntity ticker in tickerList)
            {
                if (stockList.Exists((stock) =>
                {
                    return stock.Code == ticker.T;
                }))
                {

                    if (!this.stockDict.ContainsKey(ticker.T))
                    {
                        this.stockDict.Add(ticker.T, new List<TickerEntity>());
                    }

                    this.stockDict[ticker.T].Add(ticker);
                }
            }
        }

        public List<TickerEntity> LoadTickerFromCSV(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var tickers = new List<TickerEntity>();
                // csv.Read();
                while (csv.Read())
                {
                    try
                    {
                        var ticker = new TickerEntity();

                        ticker.T = csv.GetField<string>(0);
                        ticker.P = csv.GetField<int>(1);
                        ticker.O = csv.GetField<float>(2);
                        ticker.H = csv.GetField<float>(3);
                        ticker.L = csv.GetField<float>(4);
                        ticker.C = csv.GetField<float>(5);
                        ticker.V = csv.GetField<long>(6);

                        tickers.Add(ticker);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error load csv {ex.Message}, path: {path}");
                    }
                }

                return tickers;
            }
        }
    }
}