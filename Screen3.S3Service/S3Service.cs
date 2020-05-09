using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Screen3.Utils;
using System.Text;

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

                string fileName = FileHelper.GetFileNameFromKey(keyName);

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                String path = targetPath + fileName;

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    ObjectHelper.CopyStream(responseStream, fs);
                    fs.Flush();

                    downloadedFile = path;
                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when downloading an object, keyname {1}", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when downloading an object, keyname {1}", e.Message);
            }

            return downloadedFile;
        }

        public async Task<String> DownloadContentFromS3Async(string bucketName, string keyName)
        {
            StringBuilder content = new StringBuilder();

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    content.Append(reader.ReadToEnd()); // Now you process the response body.
                }

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when downloading from S3, keyname {1}", e.Message, keyName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when downloading from S3, keyname {1}", e.Message, keyName);
            }

            return content.ToString();
        }

        public async Task UploadStringContentToS3Async(string bucketName, string keyName, string content)
        {
            var fileTransferUtility = new TransferUtility(client);

            using (var streamToUpload = ObjectHelper.GenerateStreamFromString(content))
            {
                await fileTransferUtility.UploadAsync(streamToUpload,
                                           bucketName, keyName);
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

        public async Task CopyObject(string srcBuctet, string srcKey, string destBucket, string destKey) {
            try
            {
                CopyObjectRequest request = new CopyObjectRequest
                {
                    SourceBucket = srcBuctet,
                    SourceKey = srcKey,
                    DestinationBucket = destBucket,
                    DestinationKey = destKey
                };
                CopyObjectResponse response = await client.CopyObjectAsync(request);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when copying an object, keyname {1}", e.Message, srcKey);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when copying an object, keyname {1}", e.Message, srcKey);
            }
        }

        public async Task DeleteObject(string bucketName, string keyName) {
           try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                await client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when deleting an object", e.Message, keyName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message, keyName);
            }
        }
    }
}
