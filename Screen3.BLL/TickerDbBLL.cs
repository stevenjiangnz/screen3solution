using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using System.Text;
using Screen3.Utils;
using Screen3.DynamoService;

namespace Screen3.BLL
{
    public class TickerDbBLL
    {
        private TickerServiceDAL tickerService;

        public TickerDbBLL(string tableName)
        {
            this.tickerService = new TickerServiceDAL(tableName);
        }

        public async Task AppendTickerRange(List<TickerEntity> dayTickerList)
        {
            List<TickerEntity> writtingList  = new List<TickerEntity>();
            for (int i =0; i< dayTickerList.Count; i++) {
                writtingList.Add(dayTickerList[i]);
                if (i == dayTickerList.Count -1 || i % 100 ==0 ) {
                    if (writtingList.Count > 0) {
                        await this.tickerService.UpdateBatch(writtingList);
                        Console.WriteLine($"write to db for {writtingList[0].T} {i}");
                        writtingList.Clear();            
                    }
                }
                
            }

            return; 
        }
    }
}
