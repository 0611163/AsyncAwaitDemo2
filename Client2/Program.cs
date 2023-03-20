using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client2：使用的全部是最新的异步语法
 * 
 */

/**
 * 
 * Client、Client2、Client5对比
 * 1. 总耗时都差不多
 * 2. Client2不需要大线程池，Client、Client5需要大线程池，如果是小线程池耗时长
 * 
 */

await testService.RunTest(async (url, i) =>
{
    return await QueryAsync($"{url}?i={i}");
});

Console.ReadLine();

#region QueryAsync
async Task<string> QueryAsync(string url)
{
    HttpClient httpClient = HttpClientFactory.GetClient();
    var result = await (await httpClient.GetAsync(url)).Content.ReadAsStringAsync();
    return result;
}
#endregion
