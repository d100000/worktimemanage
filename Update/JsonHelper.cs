using System;
using Newtonsoft.Json;

namespace Update
{
    /// <summary>
    /// Json操作类
    /// </summary>
    public class JsonHelper
    {
        public static T Deserialize<T>(string json) where T : new()
        {
            json = json.Replace("\r\n", "");// 去除非法字段
            T t = new T();
            if (json.Contains(@"""data"":false"))// 处理错误返回信息，防止反序列化错误
            {
                json = json.Replace(@"""data"":false", @"""data"":{}");
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // TODO:错误处理
                return new T();
            }
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
