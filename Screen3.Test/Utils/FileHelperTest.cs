using System;
using System.Collections.Generic;
using Xunit;
using Screen3.DynamoService;
using Screen3.Entity;
using Screen3.BLL;
using Screen3.Utils;

namespace Screen3.Test.Utils
{
    public class FileHelperTest
    {
        [Fact]
        public void TestDirSearch(){
            var fileList = FileHelper.DirSearch("/tmp/screen3_temp_files/originExtractedFiles/source/");

            Console.WriteLine("file list: " + ObjectHelper.ToJson(fileList));
        }

      }
}