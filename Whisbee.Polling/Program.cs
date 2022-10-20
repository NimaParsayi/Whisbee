using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Whisbee.Controllers;

var bot = new TelegramBotClient("5711953624:AAHSS5F_y3t6_Hk7O3BK0-9qTABjkqeS9Pw");
using var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = new[] { UpdateType.Message, UpdateType.InlineQuery, UpdateType.CallbackQuery, UpdateType.ChosenInlineResult },
    ThrowPendingUpdates = true
};

await bot.ReceiveAsync(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Console.WriteLine(update.Type);
    if (update.Type == UpdateType.Message)
        await new HandleMessages().CheckMessage(botClient, update?.Message);
    else if (update.Type == UpdateType.InlineQuery)
        await new HandleInlineQueries().CheckText(botClient, update?.InlineQuery);
    else if (update.Type == UpdateType.CallbackQuery)
        await new HandleCallbackQueries().CheckText(botClient, update?.CallbackQuery);
    else if (update.Type == UpdateType.ChosenInlineResult)
        await new HandleChosenInlineResults().CheckResult(botClient, update?.ChosenInlineResult);
}

static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    if (exception is ApiRequestException apiRequestException)
    {
        await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
    }
}