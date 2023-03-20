using System.Diagnostics;
using Utils;

namespace ClientCommon
{
    /// <summary>
    /// 测试服务类
    /// </summary>
    public class TestService
    {
        string _url = "http://localhost:5028/Test/Query";

        StatisticsUtil _statUtil = new StatisticsUtil();

        public async Task Start()
        {
            await _statUtil.Start();
        }

        public async Task RunTest(Func<string, int, Task<string>> func)
        {
            await Task.Delay(500);

            _statUtil.Log("开始");
            Stopwatch sw1 = Stopwatch.StartNew();
            await Task.Run(async () =>
            {
                await Parallel.ForEachAsync(Enumerable.Range(0, 1000), new ParallelOptions() { MaxDegreeOfParallelism = 100 }, async (i, c) =>
                {
                    Stopwatch sw2 = Stopwatch.StartNew();

                    var result = await func(_url, i);

                    sw2.Stop();
                    _statUtil.Log($"请求 {i,-6} 耗时：{sw2.Elapsed.TotalSeconds:0.00}，结果：{result}");
                });
            });
            _statUtil.Log($"结束，耗时：{sw1.Elapsed.TotalSeconds:0.00}");
        }

    }
}
