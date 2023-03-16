using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 统计
    /// </summary>
    public class StatisticsUtil
    {
        private Timer _timer;

        private int _count;

        private int _sendCount;

        private ConcurrentQueue<string> _msg = new ConcurrentQueue<string>();

        public async Task Start()
        {
            _timer = new Timer(o =>
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    while (_msg.TryDequeue(out string msg))
                    {
                        sb.Append($"{msg}\r\n");
                    }
                    sb.Append($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 当前已处理请求数量：{_count}，当前已发送请求数量：{_sendCount}");
                    Console.WriteLine(sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex}");
                }
            }, null, 2000, 2000);

            await Task.CompletedTask;
        }

        /// <summary>
        /// 计数加1
        /// </summary>
        public void AddOne()
        {
            Interlocked.Increment(ref _count);
        }

        /// <summary>
        /// 计数加1
        /// </summary>
        public void AddOneSendCount()
        {
            Interlocked.Increment(ref _sendCount);
        }

        /// <summary>
        /// 添加Log
        /// </summary>
        public void AddInfo(string msg)
        {
            _msg.Enqueue(msg);
        }
    }
}
