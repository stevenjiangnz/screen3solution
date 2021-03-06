﻿using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using System.Text;
using Screen3.Utils;
using Screen3.S3Service;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace Screen3.BLL
{
    public class TickerBLL
    {
        private string S3_Bucket_Name;
        private string localFolder;
        private int local_ticker_ttL = 12; // 12 hours

        public TickerBLL(string bucketName, string localFolder)
        {
            this.S3_Bucket_Name = bucketName;
            this.localFolder = localFolder;
        }

        public async Task<List<TickerEntity>> GetWeeklyTickerEntityList(string code, int? start = 0, int? end = 0)
        {
            List<TickerEntity> weeklyTickerList = null;
            int weekStart = 0;
            int weekEnd = 0;

            if (start != 0)
            {
                weekStart = DateHelper.ToInt(DateHelper.ToDate(start.Value).AddDays(-7));
            }

            if (end != 0)
            {
                weekEnd = DateHelper.ToInt(DateHelper.ToDate(end.Value).AddDays(7));
            }
            List<TickerEntity> dayList = await this.GetDailyTickerEntityList(code, weekStart, weekEnd);

            weeklyTickerList = this.GetWeeklyTickerListFromDayList(dayList);

            return weeklyTickerList.Where(t => (start == 0 || t.P >= start) && (end == 0 || t.P <= end)).ToList();
        }


        public async Task<List<TickerEntity>> GetDailyTickerEntityList(string code, int? start = 0, int? end = 0)
        {
            List<TickerEntity> tickerList = null;
            string localTickerFilePath = $@"{localFolder}{code}/{code}_day.txt";
            bool isLocalAvailable = false;

            Directory.CreateDirectory($@"{localFolder}{code}/");
            if (File.Exists(localTickerFilePath))
            {
                FileInfo tickerFile = new FileInfo(localTickerFilePath);

                if (tickerFile.CreationTime > DateTime.Now.AddHours(this.local_ticker_ttL * -1))
                {
                    isLocalAvailable = true;
                }
                else
                {
                    File.Delete(localTickerFilePath);
                }
            }

            if (isLocalAvailable)
            {
                Console.WriteLine("Screen3: load ticker form local");
                string content = File.ReadAllText(localTickerFilePath);
                tickerList = this.getTickerListFromString(content);
            }
            else
            {
                Console.WriteLine("Screen3: load ticker form S3");

                tickerList = await this.GetExistingDayTickersFromS3(code);
                this.SaveTickerlistToLocal(localTickerFilePath, tickerList);
            }

            var filteredList = tickerList.Where(t => (start == 0 || t.P >= start) && (end == 0 || t.P <= end)).ToList();

            return filteredList;
        }

        public void SaveTickerlistToLocal(string path, List<TickerEntity> tickerList)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var ticker in tickerList)
            {
                sb.AppendLine(ticker.ToString());
            }

            File.WriteAllText(path, sb.ToString());
        }

        public async Task SaveTickersToS3(string code, List<TickerEntity> tickerList, Boolean mergeExisting = false)
        {
            string keyName = $@"ticker/{code}/{code}_day.txt";
            string content = string.Empty;
            List<TickerEntity> mergedList = null;
            if (mergeExisting)
            {
                List<TickerEntity> existingList = await this.GetExistingDayTickersFromS3(code);

                var comparer = new TickerComparer();
                mergedList = tickerList.Union(existingList, comparer).ToList();
            }
            else
            {
                mergedList = tickerList;
            }

            List<TickerEntity> sortedList = mergedList.OrderBy(o => o.P).ToList();
            foreach (TickerEntity t in sortedList)
            {
                content = content + t.ToString() + "\n";
            }

            S3Service.S3Service service = new S3Service.S3Service();
            await service.UploadStringContentToS3Async(this.S3_Bucket_Name, keyName, content);
        }


        public List<TickerEntity> GetWeeklyTickerListFromDayList(List<TickerEntity> dayTickerList)
        {
            List<TickerEntity> weeklyTickerList = new List<TickerEntity>();
            Dictionary<int, List<TickerEntity>> tickerDict = new Dictionary<int, List<TickerEntity>>();

            foreach (TickerEntity ticker in dayTickerList)
            {
                int beginOfWeek = DateHelper.BeginOfWeek(ticker.P);
                if (!tickerDict.ContainsKey(beginOfWeek))
                {
                    tickerDict.Add(beginOfWeek, new List<TickerEntity>());
                }

                tickerDict[beginOfWeek].Add(ticker);
            }

            foreach (var item in tickerDict)
            {
                TickerEntity weeklyTicker = this.GetWeeklyTickerFromDayList(item.Key, item.Value);

                if (weeklyTicker != null)
                {
                    weeklyTickerList.Add(weeklyTicker);
                }
            }

            return weeklyTickerList;
        }

        public TickerEntity GetWeeklyTickerFromDayList(int period, List<TickerEntity> dayTickerList)
        {
            TickerEntity weeklyTicker = null;

            if (dayTickerList.Count > 0)
            {
                TickerEntity[] tickerArray = dayTickerList.OrderBy(o => o.P).ToArray();

                for (int i = 0; i < tickerArray.Length; i++)
                {
                    if (i == 0)
                    {
                        weeklyTicker = tickerArray[i];
                    }
                    else
                    {
                        if (weeklyTicker.H < tickerArray[i].H)
                            weeklyTicker.H = tickerArray[i].H;

                        if (weeklyTicker.L > tickerArray[i].L)
                            weeklyTicker.L = tickerArray[i].L;

                        if (i == tickerArray.Length - 1)
                        {
                            weeklyTicker.C = tickerArray[i].C;
                            weeklyTicker.P = period;
                        }

                        weeklyTicker.V += tickerArray[i].V;
                    }
                }
            }

            return weeklyTicker;
        }
        public async Task<List<TickerEntity>> GetExistingDayTickersFromS3(string code)
        {
            List<TickerEntity> tickerList = new List<TickerEntity>();
            string keyName = $@"ticker/{code}/{code}_day.txt";
            string content = string.Empty;

            S3Service.S3Service service = new S3Service.S3Service();

            content = await service.DownloadContentFromS3Async(this.S3_Bucket_Name, keyName);

            tickerList = this.getTickerListFromString(content);

            return tickerList;
        }

        public List<TickerEntity> getTickerListFromString(string content)
        {
            List<TickerEntity> tickers = new List<TickerEntity>();

            var tickerLines = StringHelper.SplitToLines(content);

            foreach (string line in tickerLines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                {
                    tickers.Add(new TickerEntity(line));
                }
            }

            return tickers;
        }


        public async Task GetTickerFromEmail(string emailAccount, string emailPwd, string bucketName)
        {
            List<string> fileNames = new List<string>();
            string localInboxFolder = "/tmp/screen3_temp_files/inbox/";
            string localInboxZipFolder = "/tmp/screen3_temp_files/inbox_zip/";
            string zipFileName = "";
            
            FileHelper.ClearDirectory(localInboxFolder, true);
            FileHelper.ClearDirectory(localInboxZipFolder, true);
            
            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(emailAccount, emailPwd);

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                foreach (var uid in inbox.Search(SearchQuery.NotSeen))
                {
                    var message = inbox.GetMessage(uid);
                    if (message.Subject.IndexOf("Daily Historical Data") >= 0)
                    {
                        foreach (MimeEntity attachment in message.Attachments)
                        {
                            var fileName = localInboxFolder + attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;

                            using (var stream = File.Create(fileName))
                            {
                                if (attachment is MessagePart)
                                {
                                    var rfc822 = (MessagePart)attachment;
                                    rfc822.Message.WriteTo(stream);
                                }
                                else
                                {
                                    var part = (MimePart)attachment;
                                    part.Content.DecodeTo(stream);
                                }
                            }
                            fileNames.Add(fileName);
                        }
                    }

                    inbox.AddFlags(uid, MessageFlags.Seen, true);
                }

                client.Disconnect(true);

                if (fileNames.Count > 0) {
                    S3Service.S3Service service = new S3Service.S3Service();

                    zipFileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".zip";
                    var zipFilePath = localInboxZipFolder + zipFileName;
                    
                    ZipFile.CreateFromDirectory(localInboxFolder, zipFilePath);

                    await service.UploadFileToS3Async(bucketName, "source/" + zipFileName, zipFilePath);
                }
            }
        }
    }
}
