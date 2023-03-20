using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// HTTP工具类
    /// </summary>
    public class HttpUtil
    {
        #region HttpGet
        /// <summary>
        /// HttpGet
        /// note: 这是一个同步HTTP请求方法
        /// </summary>
        public static string HttpGet(string url)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                stream.Close();
                return content;
            }
            catch
            {
                Console.WriteLine($"HttpUtil.HttpGet GET请求 错误 URL：{url}");
                throw;
            }
        }
        #endregion

    }
}
