using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;

namespace Whisbee.Polling.Abstract;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService
    where TUpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;

    internal ReceiverServiceBase(
        ITelegramBotClient botClient,
        TUpdateHandler updateHandler)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
    }

    /// <summary>
    /// Start to service Updates with provided Update Handler class
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        // ToDo: we can inject ReceiverOptions through IOptions container
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
            ThrowPendingUpdates = true,
        };

        // Start receiving updates
        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}