using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace screen3_data_loader
{
    public class S3Service
    {
        private const string bucketName = "stevenjiangnz-screen3-eod-source";
        private const string keyName = "1997-2006.zip";
        private const string tempFolder = "/tmp/screen3_temp_files/";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast2;
        private static IAmazonS3 client;

        public async Task<String> Connect()
        {
            client = new AmazonS3Client(bucketRegion);
            var result = await this.ReadObjectDataAsync();
            return result;
        }


        public async Task<String> ReadObjectDataAsync()
        {
            string responseBody = "";
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                String path = tempFolder + keyName;

                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    this.CopyStream(responseStream, fs);
                    fs.Flush();

                }

                FileInfo fi = new FileInfo(path);

                Console.WriteLine($"File info fullname: {fi.FullName}  size: {fi.Length}");

                ZipFile.ExtractToDirectory(path, tempFolder + "/extractedFiles/");

                this.DirSearch(tempFolder + "/extractedFiles/");


                return responseBody;

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return null;
            }
        }

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
    }
}