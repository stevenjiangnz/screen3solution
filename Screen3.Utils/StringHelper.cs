using System;
using System.IO;
using System.Collections.Generic;

namespace Screen3.Utils
{
    public class StringHelper
    {
        public static List<String> SplitToLines(string inputText)
        {
            List<String> lineList = new List<string>();
            string[] lines = inputText.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );

            for (int i = 0; i < lines.Length; i++)
            {
                if (!String.IsNullOrEmpty(lines[i]))
                {
                    lineList.Add(lines[i]);
                }
            }
            return lineList;
        }
    }
}
