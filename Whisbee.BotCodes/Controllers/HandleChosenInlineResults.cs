using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

                text = $"🔒 این یک پیام محرمانه به *{username}* از طرف *{result.From.FirstName}* است که فقط برای این دو شخص قابل مشاهده است !" +
                           $"\n\n🐝 ویسبی؛ ساده ترین ربات ارسال پیام محرمانه در تلگرام";

                await botClient.EditMessageTextAsync(result.InlineMessageId, text, ParseMode.Markdown, replyMarkup: default);
            }
        }
    }
}