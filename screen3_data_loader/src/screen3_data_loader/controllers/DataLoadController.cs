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
using Screen3.Utils;

namespace screen3_data_loader.controllers
{
    public class DataLoadController
    {

        private string S3_Bucket_Name;
        private string Temp_Folder;

        public DataLoadController()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
            this.Temp_Folder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
        }
        public async Task<String> StartProcessAsync()
        {
            string result = "";

            List<S3Object> fileList = await this.GetSourceFileListAsync(this.S3_Bucket_Name, "source");

            LambdaLogger.Log($"found files in the source folder, {ObjectHelper.ToJson(fileList)}\n");

            foreach (var fileInfo in fileList) {
                if (fileInfo.Size > 0) {
                    LambdaLogger.Log($"About to download file, {fileInfo.Key}\n");
                    String resultFileName = await this.DownloadFileAsync(fileInfo.BucketName, fileInfo.Key, this.Temp_Folder + "originSourceFiles/");

                    string dailyTargetFolder = this.Temp_Folder + $"originExtractedFiles/{fileInfo.Key}/";
                    LambdaLogger.Log($"Download file, {fileInfo.Key}, about to extract to {dailyTargetFolder}.\n");
                    var dailyFileList = this.ExtractIntoDayData(resultFileName, dailyTargetFolder);
                    LambdaLogger.Log($"Extracted files done. {ObjectHelper.ToJson(dailyFileList)}\n");
                }
            }

            return result;
        }

        public List<String>  ExtractIntoDayData(string fileName, string targetPath) {
            List<String> resultFileList = new List<String>();

            S3Helper.ClearDirectory(targetPath, true);

            ZipFile.ExtractToDirectory(fileName, targetPath);

            resultFileList = S3Helper.DirSearch(targetPath);

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

    }
}