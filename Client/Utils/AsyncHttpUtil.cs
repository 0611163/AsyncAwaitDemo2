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
        private string _url;

        private DateTime _startTime;

        private IAsyncResult _asyncResult;

        /// <summary>
        /// 异步HTTP工具类
        /// </summary>
        /// <param name="url">URL</param>
        public AsyncHttpUtil(string url)
        {
            _url = url;
        }

        /// <summary>
        /// 开始异步GET请求
        /// </summary>
        public Task BeginGetAsync()
        {
            _startTime = DateTime.Now;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "GET";
            _asyncResult = request.BeginGetResponse(null, request);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 结束异步GET请求
        /// </summary>
        public async Task<string> EndGetAsync()
        {
            while (true) // note: 等待返回结果
            {
                if (DateTime.Now.Subtract(_startTime).TotalSeconds > 180) //超时
                {
                    break;
                }
                else if (_asyncResult.IsCompleted) //已完成
                {
                    break;
                }
                else //未完成
                {
                    await Task.Delay(1);
                }
            }

            HttpWebRequest request = (HttpWebRequest)_asyncResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(_asyncResult);
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            string content = sr.ReadToEnd();
            stream.Close();
            return content;
        }
    }
}
