using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Whisbee.Controllers;

namespace Whisbee
{
    public static class WhisbeeControl
    {
        private static ITelegramBotClient bot;
        public static async Task Start()
        {
            bot = new TelegramBotClient("5548331573:AAFlxj28xDPwasXdcmg6ZUhZBocCOAgWy78");
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
}