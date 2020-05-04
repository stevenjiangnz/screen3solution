using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Lambda.Core;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using screen3_data_loader.utils;

namespace screen3_data_loader.services
{
    public class S3Service
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static IAmazonS3 client;
        private string tempFolder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER");
  
        public S3Service()
        {
            client = new AmazonS3Client(bucketRegion);
            // Create folder
            Directory.CreateDirectory(tempFolder);

        }

        public async Task<String> DownloadFileFromS3Async(string bucketName, string keyName, string targetPath)
        {
            String downloadedFile = String.Empty;

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                string fileName = S3Helper.GetFileNameFromKey(keyName);

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                String path = targetPath + fileName;

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    this.CopyStream(responseStream, fs);
                    fs.Flush();

                    downloadedFile = path;
                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

            return downloadedFile;
        }
        // public async Task<String> ReadObjectDataAsync()
        // {
        //     string tempFolder = this.tempFolder;
        //     string responseBody = "";
        //     try
        //     {
        //         GetObjectRequest request = new GetObjectRequest
        //         {
        //             BucketName = "bucketName",
        //             Key = "keyName"
        //         };
        //         String path = tempFolder + "keyName";

        //         if (!Directory.Exists(tempFolder))
        //         {
        //             Directory.CreateDirectory(tempFolder);
        //         }

        //         using (GetObjectResponse response = await client.GetObjectAsync(request))
        //         using (Stream responseStream = response.ResponseStream)
        //         using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        //         {
        //             this.CopyStream(responseStream, fs);
        //             fs.Flush();

        //         }

        //         FileInfo fi = new FileInfo(path);

        //         Console.WriteLine($"File info fullname: {fi.FullName}  size: {fi.Length}");

        //         ZipFile.ExtractToDirectory(path, tempFolder + "/extractedFiles/");

        //         this.DirSearch(tempFolder + "/extractedFiles/");


        //         return responseBody;

        //     }
        //     catch (AmazonS3Exception e)
        //     {
        //         Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
        //         return null;
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        //         return null;
        //     }
        // }

        public void DirSearch(string sDir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        Console.WriteLine(f);
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
        private void CopyStream(Stream src, Stream dest)
        {
            int _bufferSize = 4096;
            var buffer = new byte[_bufferSize];
            int len;
            while ((len = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, len);
            }
        }

        public async Task<List<S3Object>> ListingObjectsAsync(String bucketName, String prefix)
        {
            List<S3Object> fileList = new List<S3Object>();
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    MaxKeys = 10
                };

                if (!String.IsNullOrEmpty(prefix))
                {
                    request.Prefix = prefix;
                }

                ListObjectsV2Response response;
                do
                {
                    response = await client.ListObjectsV2Async(request);

                    foreach (S3Object entry in response.S3Objects)
                    {
                        fileList.Add(entry);
                    }
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                LambdaLogger.Log("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
            catch (Exception e)
            {
                LambdaLogger.Log("Exception: " + e.ToString());
            }

            return fileList;
        }
    }

}
