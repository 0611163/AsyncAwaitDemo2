using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client、Client2-Client6
 * 1. Client3、Client4批量请求接口的吞吐量不行，耗时长
 * 2. Client5意义不大，可以用Client6代替
 * 3. 但是Client6需要独立的大线程池
 * 4. Client2使用的全部是最新的异步语法，一般情况下使用Client2就可以了
 * 5. 这些demo，最核心的是Client，它可以把回调函数改造成异步，但是需要一个独立的大线程池
 * 
 */

/**
 * 
 * Client：意义是把回调包装成异步，但是需要一个独立的大线程池
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
