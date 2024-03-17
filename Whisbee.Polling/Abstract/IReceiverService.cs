namespace Whisbee.Polling.Abstract;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}