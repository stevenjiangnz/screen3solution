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
            string keyName = $@"ticker/{code}/{code}_day.txt";

            string content = "";

            foreach(TickerEntity t in tickerList) {
                content = content + t.ToString() + "\n";
            }

            S3Service.S3Service service = new S3Service.S3Service();
            await service.UploadStringContentToS3Async(this.S3_Bucket_Name, keyName, content );
        }
    }
}
