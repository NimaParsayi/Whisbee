using Microsoft.Extensions.Logging;
using Whisbee.Polling.Abstract;

namespace Whisbee.Polling.Services;

// Compose Polling and ReceiverService implementations
public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}