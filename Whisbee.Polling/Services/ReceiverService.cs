using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Whisbee.Polling.Abstract;

namespace Whisbee.Polling.Services;

// Compose Receiver and UpdateHandler implementation
public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler)
        : base(botClient, updateHandler)
    {
    }
}