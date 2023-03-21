using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 异步HTTP工具类
    /// </summary>
    public class AsyncHttpUtil
    {
        /// <summary>
        /// 异步GET请求
        /// </summary>
        public static async Task<string> GetAsync(string url)
        {
            DateTime startTime = DateTime.Now;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            IAsyncResult asyncResult = request.BeginGetResponse(null, request);

            while (true) // note: 等待返回结果
            {
                if (DateTime.Now.Subtract(startTime).TotalSeconds > 180) //超时
                {
                    break;
                }
                else if (asyncResult.IsCompleted) //已完成
                {
                    break;
                }
                else //未完成
                {
                    await Task.Delay(1);
                }
            }

            request = (HttpWebRequest)asyncResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string content = sr.ReadToEnd();
            stream.Close();
            return content;
        }
    }
}
