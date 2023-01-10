using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Whisbee.Controllers
{
    public class HandleCallbackQueries
    {
        public async Task CheckText(ITelegramBotClient bot, CallbackQuery query)
        {
            if (query.Data.StartsWith("show"))
            {
                var queryText = query.Data?.Replace("show", null);
                var querySplit = queryText.Split('-');
                var fromId = querySplit[0];
                var toId = querySplit[1];
                var message = querySplit[2];

                if (query.From.Id == long.Parse(fromId) || query.From.Id.ToString() == toId || (!string.IsNullOrEmpty(query.From.Username) && query.From.Username.ToLower() == toId.ToLower()))
                {
                    //// Original -1001711216736
                    //// Test -1001140735044
                    var groupId = -1001711216736;
                    var msg = await bot.ForwardMessageAsync(groupId, groupId, int.Parse(message));
                    var text = msg.Text.Split(':');
                    await bot.AnswerCallbackQueryAsync(query.Id, text[1], true);
                    await bot.DeleteMessageAsync(groupId, msg.MessageId);
                    await bot.SendTextMessageAsync(groupId, $"{query.From.FirstName} Seen !",
                        replyToMessageId: int.Parse(message));
                }
                else
                {
                    var text = $"🔒 متاسفم! این پیام برای تو ارسال نشده :(";
                    await bot.AnswerCallbackQueryAsync(query.Id, text, true);
                }
            }
        }
    }
}
