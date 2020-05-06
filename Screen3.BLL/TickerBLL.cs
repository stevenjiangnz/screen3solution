using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using Screen3.S3Service;

namespace Screen3.BLL
{
    public class TickerBLL
    {
        private string S3_Bucket_Name;

        public TickerBLL()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
        }

        public async Task SaveTickers(string code, List<TickerEntity> tickerList)
        {
            string localTickerFolder = Environment.GetEnvironmentVariable("SCREEN3_TEMP_FOLDER") + $@"localticker/{code}/";
            string fileName = localTickerFolder + code + "_day.txt";

            // Save the ticker to local
            Directory.CreateDirectory(localTickerFolder);

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                foreach (TickerEntity ticker in tickerList)
                {
                    outputFile.WriteLine(ticker.ToString());
                }
            }

            S3Service.S3Service service = new S3Service.S3Service();
            await service.UploadFileToS3Async(this.S3_Bucket_Name, $@"ticker/{code}/{code}_day.txt", fileName );
        }
    }
}
