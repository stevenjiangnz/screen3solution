using System;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Screen3.Utils
{
    public class ObjectHelper
    {
        public static string ToJson(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            return json;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static void CopyStream(Stream src, Stream dest)
        {
            int _bufferSize = 4096;
            var buffer = new byte[_bufferSize];
            int len;
            while ((len = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dest.Write(buffer, 0, len);
            }
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}
