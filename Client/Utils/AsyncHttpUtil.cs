using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 异步HTTP工具类
    /// </summary>
    public class AsyncHttpUtil
    {
        /// <summary>
        /// 线程池
        /// </summary>
        private static readonly TaskSchedulerEx _task = new TaskSchedulerEx(0, 100);

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
        public Task<string> EndGetAsync()
        {
            // note: 把_task.Run修改为Task.Run试试？思考为什么此处使用Task.Run不行？
            return _task.Run(() =>
            {
                SpinWait spinWait = new SpinWait();
                while (true) // note: 这里通过while(true)和SpinWait，等待返回结果
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
                        spinWait.SpinOnce();
                    }
                }

                HttpWebRequest request = (HttpWebRequest)_asyncResult.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(_asyncResult);
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream, Encoding.UTF8);
                string content = sr.ReadToEnd();
                stream.Close();
                return content;
            });
        }
    }
}
