
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace screen3_data_loader.utils
{
    public class S3Helper
    {
        public static string GetFileNameFromKey (string key) {
            string fileName;

            if (key.LastIndexOf("/")< 0) {
                fileName = key;
            } else {
                fileName = key.Substring(key.LastIndexOf("/") + 1);
            }

            return fileName;
        }
        
    }

}
