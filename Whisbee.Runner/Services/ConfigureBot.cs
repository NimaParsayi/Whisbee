using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Whisbee.Runner.Services
{
    public class ConfigureBot : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await WhisbeeControl.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // 
        }
    }
}
