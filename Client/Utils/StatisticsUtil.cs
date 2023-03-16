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
                        sb.Append($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} {msg}").Append("\r\n");
                    }
                    if (sb.Length > 0)
                    {
                        Console.WriteLine(sb.ToString().TrimEnd('\r').TrimEnd('\n'));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex}");
                }
            }, null, 50, 50);

            await Task.CompletedTask;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        public void Log(string msg)
        {
            _msg.Enqueue(msg);
        }
    }
}
