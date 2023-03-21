using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client7：和Client对比，Client没有使用线程池，Client7使用了线程池
 * 1. 和Client对比，Client7没有意义了
 * 
 */

await testService.RunTest(async (url, i) =>
{
    var asyncUtil = new AsyncHttpUtil($"{url}?i={i}");
    await asyncUtil.BeginGetAsync();
    return await asyncUtil.EndGetAsync();
});

Console.ReadLine();
