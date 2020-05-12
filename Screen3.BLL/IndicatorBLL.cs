using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Screen3.Entity;
using System.Linq;
using System.Text;
using Screen3.Utils;

namespace Screen3.BLL
{
    public class IndicatorBLL
    { 
        private string S3_Bucket_Name;

        public IndicatorBLL(string bucketName) {
            this.S3_Bucket_Name = bucketName;
        }


    }
}