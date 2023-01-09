using Whisbee.PollingRunner.Abstract;

namespace Whisbee.PollingRunner.Services;
public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}
