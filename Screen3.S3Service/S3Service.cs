using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Screen3.S3Service
{
    public class S3Service
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static IAmazonS3 client;

        public S3Service()
        {
            client = new AmazonS3Client(bucketRegion);
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

                string fileName = this.GetFileNameFromKey(keyName);

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
                Console.WriteLine("S3 error occurred. Exception: " + amazonS3Exception.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

            return fileList;
        }


        private string GetFileNameFromKey(string key)
        {
            string fileName;

            if (key.LastIndexOf("/") < 0)
            {
                fileName = key;
            }
            else
            {
                fileName = key.Substring(key.LastIndexOf("/") + 1);
            }

            return fileName;
        }
    }

}
