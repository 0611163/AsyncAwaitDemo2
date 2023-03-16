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

        public async Task Start()
        {
            _timer = new Timer(o =>
            {
                try
                {
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 当前已处理请求数量：{_count}");
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
    }
}
