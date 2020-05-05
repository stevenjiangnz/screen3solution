using System;
using Newtonsoft.Json;

namespace Screen3.Utils
{
    public class ObjectHelper
    {
        public static string ToJson(object obj) {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            return json;
        }
    }
}
