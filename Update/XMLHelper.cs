using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Update
{
    public class XMLHelper
    {
        /// <summary>
        /// 从文件夹读取xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static  class LoadXml<T> where T : new()
        {
            /// <summary>
            /// 读取文件并且返回相应的类
            /// </summary>
            /// <param name="filePath"></param>
            /// <returns></returns>
            public static  T Load(string filePath)
            {
                T t = new T();
                if (!filePath.Contains(@".xml"))
                    return t;
                if (!File.Exists(filePath))
                {
                    return t;
                }
                try
                {
                    var data = File.ReadAllText(filePath);
                    t = XmlDeserialize<T>(data);
                }
                catch(Exception ex)
                {

                }

                return t;
            }
        }

        /// <summary>
        /// 读取xml写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public bool WriteXml_string(string filePath, string xmlString)
        {
            if (!filePath.Contains(".xml"))
                return false;
            try
            {
                using (FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    
                    //创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                    sw.WriteLine(xmlString); //开始写入值
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool WriteJson_string(string filePath, string xmlString)
        {
            if (!filePath.Contains(".json"))
                return false;
            try
            {
                using (FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {

                    //创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                    sw.WriteLine(xmlString); //开始写入值
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 直接把object转为Xml写入文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xmlobject"></param>
        /// <returns></returns>
        public bool WriteXml_object(string filePath, object xmlobject)
        {
            if (!filePath.Contains(".xml"))
                return false;
            try
            {
                using (FileStream fs1 = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    //创建写入文件 
                    StreamWriter sw = new StreamWriter(fs1, Encoding.UTF8);
                    string content = XmlSerialize(xmlobject);
                    sw.WriteLine(content); //开始写入值
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region 序列化与反序列化

        /// <summary>
        /// 以【Encoding.UTF8】反序列化xml
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }


        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlSerializeInternal(stream, o, Encoding.UTF8);
                    stream.Position = 0;
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="o"></param>
        /// <param name="encoding"></param>
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                NewLineChars = "\r\n",
                Encoding = encoding,
                IndentChars = "    "
            };

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        public static class WebData
        {
            public static T LoadData<T>(string path )
            {
                Encoding encoding = Encoding.UTF8;
                WebRequest request = WebRequest.Create(path);//实例化WebRequest对象
                WebResponse response = request.GetResponse();//创建WebResponse对象
                Stream datastream = response.GetResponseStream();//创建流对象
                T resoult = default(T);
                if (datastream == null)
                {
                    return resoult;
                }
                StreamReader reader = new StreamReader(datastream, encoding);
                string responseFromServer = reader.ReadToEnd();//读取数据
                reader.Close();
                datastream.Close();
                response.Close();
                try
                {
                    return XmlDeserialize<T>(responseFromServer);
                }
                catch (Exception ex)
                {
                    return resoult;
                }
            }

        }


        #endregion





    }
}
