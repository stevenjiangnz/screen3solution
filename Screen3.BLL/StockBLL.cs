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
    public class StockBLL
    {
        private StockServiceDAL dal;
        public StockBLL(string tableName)
        {
            this.dal = new StockServiceDAL(tableName);
        }

        public Task<List<StockEntity>> GetAll()
        {
            return this.dal.GetAll();
        }
    }
}