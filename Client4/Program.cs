using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client3和Client4对比
 * 结果：
 * 1. 总耗时都很长
 * 2. Client4耗时更长
 * 
 */

await testService.RunTest(async (url, i) =>
{
    return await Task.Run(() =>
    {
        return HttpUtil.HttpGet($"{url}?i={i}");
    });
});

Console.ReadLine();
