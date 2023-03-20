using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private StatisticsUtil _statUtil;

        public TestController(StatisticsUtil statisticsUtil)
        {
            _statUtil = statisticsUtil;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<string> Query(int i)
        {
            var r = i % 3;

            if (r == 0)
            {
                await Task.Delay(50); //模拟耗时操作
            }
            else if (r == 1)
            {
                await Task.Delay(200); //模拟耗时操作
            }
            else if (r == 2)
            {
                await Task.Delay(500); //模拟耗时操作
            }

            _statUtil.AddOne();
            return "测试返回结果";
        }

    }
}
