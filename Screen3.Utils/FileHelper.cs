using System;
using System.IO;
using System.Collections.Generic;

namespace Screen3.Utils
{
    public class FileHelper
    {
        private static List<String> fileList = new List<String>();

        public static void ClearDirectory(string targetPath, bool? withCreate = false)
        {
            if (Directory.Exists(targetPath))
            {
                DirectoryInfo dir = new DirectoryInfo(targetPath);
                dir.Delete(true); 
            }

            if (withCreate.HasValue && withCreate == true)
            {
                Directory.CreateDirectory(targetPath);
            }
        }

        public static List<String> DirSearch(string sDir, bool? isInit = true)
        {
            if (isInit.HasValue && isInit == true)
            {
                fileList.Clear();
            }

            try
            {
                foreach (string f in Directory.GetFiles(sDir))
                {
                    fileList.Add(f);
                }

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    DirSearch(d, false);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }

            return fileList;
        }

        public static string GetFileNameFromKey(string key)
        {
            string fileName;

            if (key.LastIndexOf("/") < 0)
            {
                fileName = key;
            }
            else
            {
                fileName = key.Substring(key.LastIndexOf("/") + 1);
            }

            return fileName;
        }

    }
}
