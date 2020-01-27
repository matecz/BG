using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BGInfrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BGLoggerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;
                    RabbitOptions rabbitOptions = configuration.GetSection("rabbit").Get<RabbitOptions>();
                    services.AddSingleton(rabbitOptions);
                    services.AddHostedService<ConsumeRabbitMQHostedService>();
                });
    }
}