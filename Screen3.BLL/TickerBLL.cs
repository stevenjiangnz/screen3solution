using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using Screen3.S3Service;
using Screen3.Utils;

namespace Screen3.BLL
{
    public class TickerBLL
    {
        private string S3_Bucket_Name;

        public TickerBLL()
        {
            this.S3_Bucket_Name = Environment.GetEnvironmentVariable("SCREEN3_S3_BUCKET");
        }

        public async Task SaveTickers(string code, List<TickerEntity> tickerList, Boolean mergeExisting = false)
        {
            string keyName = $@"ticker/{code}/{code}_day.txt";
            string content = string.Empty;
            List<TickerEntity> mergedList = null;
            if (mergeExisting) {
                List<TickerEntity> existingList = await this.GetExistingDayTickers(code);

                var comparer = new TickerComparer<TickerEntity>();
                mergedList = tickerList.Union(existingList, comparer).ToList();
            } else {
                mergedList = tickerList;
            }

            List<TickerEntity> sortedList = mergedList.OrderBy(o=>o.Period).ToList();
            foreach (TickerEntity t in sortedList)
            {
                content = content + t.ToString() + "\n";
            }

            S3Service.S3Service service = new S3Service.S3Service();
            await service.UploadStringContentToS3Async(this.S3_Bucket_Name, keyName, content);
        }

        public async Task<List<TickerEntity>> GetExistingDayTickers(string code)
        {
            List<TickerEntity> tickerList = new List<TickerEntity>();
            string keyName = $@"ticker/{code}/{code}_day.txt";
            string content = string.Empty;

            S3Service.S3Service service = new S3Service.S3Service();

            content = await service.DownloadContentFromS3Async(this.S3_Bucket_Name, keyName);


            var tickerLines = StringHelper.SplitToLines(content);

            foreach (string line in tickerLines)
            {
                tickerList.Add(new TickerEntity(line));
            }

            return tickerList;
        }
    }
}
