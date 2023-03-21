using ClientCommon;
using Utils;

TestService testService = new TestService();
await testService.Start();

/**
 * 
 * Client8：Client的改进
 * 
 */

await testService.RunTest(async (url, i) =>
{
    return await AsyncHttpUtil.GetAsync($"{url}?i={i}");
});

Console.ReadLine();
