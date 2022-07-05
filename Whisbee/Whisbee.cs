using Telegram.Bot;
using Whisbee.Controllers;

namespace Whisbee;
public static class Whisbee
{
    private static ITelegramBotClient bot;
    public static async Task Run()
    {
        bot = new TelegramBotClient("5548331573:AAEBA_qAyFcjKcyGSUwR3Z-YWnEr2MYHTpY");
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[] { UpdateType.Message, UpdateType.InlineQuery, UpdateType.CallbackQuery, UpdateType.ChosenInlineResult },
            ThrowPendingUpdates = true
        };

        await bot.ReceiveAsync(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message)
            await new HandleMessages().CheckMessage(botClient, update.Message);
        else if (update.Type == UpdateType.InlineQuery)
            await new HandleInlineQueries().CheckText(botClient, update.InlineQuery);
        else if (update.Type == UpdateType.CallbackQuery)
            await new HandleCallbackQueries().CheckText(botClient, update.CallbackQuery);
        else if (update.Type == UpdateType.ChosenInlineResult)
            await new HandleChosenInlineResults().CheckResult(botClient, update.ChosenInlineResult);
    }

    private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ApiRequestException apiRequestException)
        {
            await botClient.SendTextMessageAsync(123, apiRequestException.ToString());
        }
    }
}
