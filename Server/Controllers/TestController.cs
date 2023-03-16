using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private Random _rnd = new Random();

        private StatisticsUtil _statUtil;

        public TestController(StatisticsUtil statisticsUtil)
        {
            _statUtil = statisticsUtil;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<List<int>> GetCounts(int n)
        {
            List<int> result = new List<int>();

            await Task.Delay(50); //模拟耗时操作

            for (int i = 0; i < n; i++)
            {
                result.Add(_rnd.Next(10, 20));
            }

            _statUtil.AddOne();

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<List<int>> GetValues(int n)
        {
            List<int> result = new List<int>();

            await Task.Delay(50); //模拟耗时操作

            for (int i = 0; i < n; i++)
            {
                result.Add(_rnd.Next(0, 100000));
            }

            _statUtil.AddOne();

            return result;
        }
    }
}
