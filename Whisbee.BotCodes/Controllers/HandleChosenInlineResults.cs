using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Whisbee.Controllers
{
    public class HandleChosenInlineResults
    {
        public async Task CheckResult(ITelegramBotClient botClient, ChosenInlineResult result)
        {
            if (result.Query.Contains("@"))
            {
                var query = result.Query.Split('@');
                var text = query.First();
                var username = query.Last();
                var msgId = result.ResultId.Split('-')[1];
                text = $"🐝 پیام جدید ! [{username}](https://t.me/{username})، یه پیام از طرف *{result.From.FirstName}* داری.";

                await botClient.EditMessageTextAsync(result.InlineMessageId, text, ParseMode.Markdown, replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("👀 خوندن پیام", $"show{result.From.Id}-{username}-{msgId}"),
                        InlineKeyboardButton.WithUrl("🐝 کانال ویسبی", "https://WhisbeeNews.t.me"),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("وضعیت پیام: ❌ خونده نشده", "null"), 
                    }
                }), disableWebPagePreview: true);
            }
        }
    }
}