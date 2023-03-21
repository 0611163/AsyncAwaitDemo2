using System;
using System.Collections;
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
        /// 线程池
        /// note: 对比测试：把new TaskSchedulerEx(0, 100)修改为new TaskSchedulerEx(0, 20)，然后对比总耗时
        /// note: 测试结果：线程池越大，总耗时越短
        /// </summary>
        private static readonly TaskSchedulerEx _task = new TaskSchedulerEx(0, 100);

        private string _url;

        private DateTime _startTime;

        private string _result;

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

            _task.Run(() => // note: 把同步方法包装成异步
            {
                _result = HttpUtil.HttpGet(_url);
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// 结束异步GET请求
        /// </summary>
        public async Task<string> EndGetAsync()
        {
            while (true)
            {
                if (DateTime.Now.Subtract(_startTime).TotalSeconds > 180) //超时
                {
                    break;
                }
                else if (_result != null) //已完成
                {
                    break;
                }
                else //未完成
                {
                    await Task.Delay(1);
                }
            }

            return _result;
        }
    }
}
