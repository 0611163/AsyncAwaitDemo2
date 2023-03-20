using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client5：意义是把同步包装成异步，但可能需要独立线程池，而且是大线程池
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
    var asyncUtil = new AsyncHttpUtil($"{url}?i={i}");
    await asyncUtil.BeginGetAsync();
    return await asyncUtil.EndGetAsync();
});

Console.ReadLine();
