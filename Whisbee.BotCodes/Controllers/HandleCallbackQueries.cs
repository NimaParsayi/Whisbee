using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace Whisbee.Controllers
{
    public class HandleCallbackQueries
    {
        public async Task CheckText(ITelegramBotClient bot, CallbackQuery query)
        {

            async Task<bool> IsUserJoinedChannel(long userId)
            {
                var result = await bot.GetChatMemberAsync(-1001762254043, userId);
                return result.Status == ChatMemberStatus.Creator || result.Status == ChatMemberStatus.Administrator ||
                       result.Status == ChatMemberStatus.Member;
            }

            if (await IsUserJoinedChannel(query.From.Id))
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
                        long integerId = 0;
                        long.TryParse(toId, out integerId);
                        if (query.From.Id == integerId || query.From.Username == toId)
                        {
                            var keyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("👀 خوندن پیام",
                                        $"show{fromId}-{toId}-{message}"),
                                    InlineKeyboardButton.WithUrl("🐝 کانال ویسبی", "https://WhisbeeNews.t.me"),
                                },
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("وضعیت پیام: ✅ خونده شده", "null"),
                                }
                            });
                            await bot.EditMessageReplyMarkupAsync(query.InlineMessageId, keyboard);
                        }
                    }
                    else
                    {
                        var text = $"🔒 متاسفم! این پیام برای تو ارسال نشده :(";
                        await bot.AnswerCallbackQueryAsync(query.Id, text, true);
                    }
                }
            }
            else
            {
                await bot.AnswerCallbackQueryAsync(query.Id, "🖐️ برای استفاده از ویسبی باید تو کانال ما عضو باشی.");
            }
        }
    }
}
