using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client4和Client6对比
 * 结果：
 * 1. Client4很慢，Client6很快，因为Client6使用的是独立的大线程池
 * 
 */

/**
 * 
 * Client5和Client6对比
 * 结果：
 * 1. 耗时差不多，Client6稍微快一点点
 * 2. Client5和Client6都需要大线程池
 * 
 */

/**
 * 
 * Client和Client6对比
 * 结果：
 * 1. 耗时差不多，Client6稍微快一点点
 * 2. Client6需要大线程池，Client不需要大线程池
 * 
 */

TaskSchedulerEx _task = new TaskSchedulerEx(0, 100);

await testService.RunTest(async (url, i) =>
{
    return await _task.Run(() =>
    {
        return HttpUtil.HttpGet($"{url}?i={i}");
    });
});

_task.Dispose();

Console.ReadLine();
