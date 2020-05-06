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
using CsvHelper;

namespace screen3_data_loader.controllers
{
    public class DataLoadController
    {

        private string S3_Bucket_Name;
        private string Temp_Folder;
        private List<StockEntity> stockList;
        private StockServiceDAL dal;


        public DataLoadController()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
            string asx300TableName = Environment.GetEnvironmentVariable("SCREEN3_ASX300_TABLE_NAME");
            this.dal = new StockServiceDAL(asx300TableName);
        }

        public async Task<String> StartProcessAsync()
        {
            this.stockList = await this.dal.GetAll();

            // retrieve all stock list.
            this.stockList = await this.dal.GetAll();

            string result = "";

            List<S3Object> fileList = await this.GetSourceFileListAsync(this.S3_Bucket_Name, "source");

            LambdaLogger.Log($"found files in the source folder, {ObjectHelper.ToJson(fileList)}\n");

            foreach (var fileInfo in fileList)
            {
                if (fileInfo.Size > 0)
                {
                    LambdaLogger.Log($"About to download file, {fileInfo.Key}\n");
                    String resultFileName = await this.DownloadFileAsync(fileInfo.BucketName, fileInfo.Key, this.Temp_Folder + "originSourceFiles/");

                    string dailyTargetFolder = this.Temp_Folder + $"originExtractedFiles/{fileInfo.Key}/";
                    LambdaLogger.Log($"Download file, {fileInfo.Key}, about to extract to {dailyTargetFolder}.\n");
                    List<String> dailyFileList = this.ExtractIntoDayData(resultFileName, dailyTargetFolder);

                    LambdaLogger.Log($"Extracted files done. {ObjectHelper.ToJson(dailyFileList)}\n");
                }
            }

            return result;
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

        public void AddDailyFileIntoStockDict(string path)
        {

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
                    var ticker = new TickerEntity();

                    ticker.code = csv.GetField<string>(0);
                    ticker.period = csv.GetField<int>(1);
                    ticker.open = csv.GetField<float>(2);
                    ticker.high = csv.GetField<float>(3);
                    ticker.low = csv.GetField<float>(4);
                    ticker.close = csv.GetField<float>(5);
                    ticker.volume = csv.GetField<long>(6);

                    tickers.Add(ticker);
                }

                return tickers;
            }
        }

    }
}