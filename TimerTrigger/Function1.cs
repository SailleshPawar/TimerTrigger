using CronExpressionDescriptor;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TimerTrigger
{
    public class Function1
    {

        private readonly ILogger<Function1> _logger;
        private readonly IConfiguration _configuartion;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public Function1(ILogger<Function1> logger, IConfiguration configuartion)
        {
            _logger = logger ?? throw new Exception(nameof(logger));
            _configuartion = configuartion ?? throw new Exception(nameof(logger));
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("%Schedule%")]TimerInfo myTimer)
        {

            _logger.LogInformation(ExpressionDescriptor.GetDescription("0 */5 * * * *"));
            _logger.LogInformation($"Key vault value for secret secret-func-name is {_configuartion["secret-func-name"]}");
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
