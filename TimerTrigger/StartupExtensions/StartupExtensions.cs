using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TimerTrigger.StartupExtensions
{
    public static class StartupExtensions
    {
      public  static void AddApplicationInsightLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(logging =>
            {
                string instrumentationKey = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(instrumentationKey))
                {
                    logging.AddApplicationInsights(instrumentationKey).SetMinimumLevel(LogLevel.Information);
                    logging.AddApplicationInsightsWebJobs(o => o.InstrumentationKey = instrumentationKey);
                }

                logging.AddAzureWebAppDiagnostics();
            });
        }
    }
}
