using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Whisbee.Controllers;

namespace Whisbee.Polling.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;

    public UpdateHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery } => BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            { ChosenInlineResult: { } chosenInlineResult } => BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Text is not { } messageText)
            return;

        await new HandleMessages().CheckMessage(_botClient, message);
    }

    // Process Inline Keyboard callback data
    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await new HandleCallbackQueries().CheckText(_botClient, callbackQuery);
    }

    #region Inline Mode

    private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery, CancellationToken cancellationToken)
    {
        await new HandleInlineQueries().CheckText(_botClient, inlineQuery);

    }

    private async Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult, CancellationToken cancellationToken)
    {
        await new HandleChosenInlineResults().CheckResult(_botClient, chosenInlineResult);
    }

    #endregion

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}