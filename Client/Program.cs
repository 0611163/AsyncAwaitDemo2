using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Diagnostics;
using Utils;

string _url = "http://localhost:5029/Test/GetValues";
string _url2 = "http://localhost:5029/Test/Query2";

StatisticsUtil statUtil = new StatisticsUtil();
await statUtil.Start();

await Task.Delay(1000);

await Task.Run(() =>
{
    Task.Run(async () =>
    {
        statUtil.Log($"查询第1个接口 开始");
        await Parallel.ForEachAsync(Enumerable.Range(0, 500), new ParallelOptions() { MaxDegreeOfParallelism = 200 }, async (i, c) =>
        {
            Stopwatch sw = Stopwatch.StartNew();

            List<int> counts = new List<int>() { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
            Tuple<ConcurrentQueue<int>, ConcurrentDictionary<int, int>> result = await GetValuesAsync(counts);
            ConcurrentQueue<int> queue = result.Item1;
            ConcurrentDictionary<int, int> dict = result.Item2;

            statUtil.Log($"查询第1个接口，结束{i}，结果数量：{queue.Count}，去重-->{dict.Count}，耗时：{sw.Elapsed.TotalSeconds:0.000}秒");
            sw.Stop();
        });
        statUtil.Log($"查询第1个接口 结束");
    });

    Thread.Sleep(500);

    Task.Run(async () =>
    {
        statUtil.Log($"查询第2个接口 开始");
        await Parallel.ForEachAsync(Enumerable.Range(0, 500), new ParallelOptions() { MaxDegreeOfParallelism = 200 }, async (i, c) =>
        {
            Stopwatch sw = Stopwatch.StartNew();

            string result = await Query2Async();

            statUtil.Log($"查询第2个接口，结束{i}，耗时：{sw.Elapsed.TotalSeconds:0.000}秒");
            sw.Stop();
        });
        statUtil.Log($"查询第2个接口 结束");
    });
});

Console.ReadLine();

#region GetValuesAsync
async Task<Tuple<ConcurrentQueue<int>, ConcurrentDictionary<int, int>>> GetValuesAsync(List<int> counts)
{
    HttpClient httpClient = HttpClientFactory.GetClient();
    string postData = JsonConvert.SerializeObject(counts);
    HttpContent content = new StringContent(postData);
    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
    var result = await (await httpClient.PostAsync(_url, content)).Content.ReadAsStringAsync();
    return JsonConvert.DeserializeObject<Tuple<ConcurrentQueue<int>, ConcurrentDictionary<int, int>>>(result);
}
#endregion

#region Query2Async
async Task<string> Query2Async()
{
    HttpClient httpClient = HttpClientFactory.GetClient();
    var result = await (await httpClient.GetAsync(_url2)).Content.ReadAsStringAsync();
    return result;
}
#endregion
