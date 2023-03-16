using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Utils;

namespace MiddleServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly string _url1 = "http://localhost:5028/Test/GetCounts";
        private readonly string _url2 = "http://localhost:5028/Test/GetValues";

        private StatisticsUtil _statUtil;

        public TestController(StatisticsUtil statisticsUtil)
        {
            _statUtil = statisticsUtil;
        }

        #region GetValues
        [HttpPost]
        [Route("[action]")]
        public async Task<Tuple<ConcurrentQueue<int>, ConcurrentDictionary<int, int>>> GetValues(List<int> counts)
        {
            Stopwatch sw = Stopwatch.StartNew();

            ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            object lockObj = new object();
            int queryCount = 0;
            await Parallel.ForEachAsync(counts, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, async (i, c) =>
            {
                List<int> counts2 = await GetCountsAsync(i);
                Interlocked.Increment(ref queryCount);
                await Parallel.ForEachAsync(counts2, new ParallelOptions() { MaxDegreeOfParallelism = 8 }, async (j, c) =>
                {
                    List<int> list3 = await GetValuesAsync(j);
                    Interlocked.Increment(ref queryCount);
                    foreach (int item in list3)
                    {
                        queue.Enqueue(item);
                        lock (lockObj)
                        {
                            if (!dict.ContainsKey(item))
                            {
                                dict.TryAdd(item, item);
                            }
                        }
                    }
                });
            });

            _statUtil.AddOne();
            _statUtil.AddInfo($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} 结束，查询次数：{queryCount}，结果数量：{queue.Count}，去重-->{dict.Count}，耗时：{sw.Elapsed.TotalSeconds:0.000}秒");
            sw.Stop();

            return new Tuple<ConcurrentQueue<int>, ConcurrentDictionary<int, int>>(queue, dict);
        }
        #endregion

        #region Query2
        [HttpGet]
        [Route("[action]")]
        public async Task<string> Query2()
        {
            await Task.Delay(50); //模拟耗时操作

            _statUtil.AddOne();
            return "测试返回结果";
        }
        #endregion

        #region GetCountsAsync
        private async Task<List<int>> GetCountsAsync(int n)
        {
            HttpClient httpClient = HttpClientFactory.GetClient();
            var result = await (await httpClient.GetAsync($"{_url1}?n={n}")).Content.ReadAsStringAsync();
            _statUtil.AddOneSendCount();
            return JsonSerializer.Deserialize<List<int>>(result);
        }
        #endregion

        #region GetValuesAsync
        private async Task<List<int>> GetValuesAsync(int n)
        {
            HttpClient httpClient = HttpClientFactory.GetClient();
            var result = await (await httpClient.GetAsync($"{_url2}?n={n}")).Content.ReadAsStringAsync();
            _statUtil.AddOneSendCount();
            return JsonSerializer.Deserialize<List<int>>(result);
        }
        #endregion

    }
}
