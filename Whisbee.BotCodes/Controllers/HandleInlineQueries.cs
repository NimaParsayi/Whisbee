using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Whisbee.Controllers
{

    public class HandleInlineQueries
    {
        public async Task CheckText(ITelegramBotClient bot, InlineQuery query)
        {
            var queryId = query.Id;
            var queryFrom = query.From;
            var queryText = query.Query;

            InlineQueryResult[] results = { };

            if (queryText.Trim() == "")
            {
                var botInGroupText = $"🐝 ویسبی | ربات ارسال پیام محرمانه در گروه" +
                    $"\n\n🤔 چطور استفاده کنم؟" +
                    $"\n- استفاده از ویسبی خیلی سادست ! کافیه توی گروه روی شخصی که میخوای ریپلای کنی بعد با نوشتن @WhisbeeBot و نوشتن پیامت کلیک روی پاپ‌آپ ارسالش کنی ! همین." +
                    $"\n\n🗞️ اخبار ویسبی:" +
                    $"\n🆔 @WhisbeeNews";
                var botNotInGroupText = $"🐝 ویسبی | ربات ارسال پیام محرمانه در گروه" +
                                     $"\n\n🤔 چطور استفاده کنم؟" +
                                     $"\n- استفاده از ویسبی خیلی سادست ! کافیه توی گروه روی شخصی که میخوای ریپلای کنی بعد با نوشتن @WhisbeeBot و نوشتن پیامت کلیک روی پاپ‌آپ ارسالش کنی ! همین." +
                                     $"\n\n🗞️ اخبار ویسبی:" +
                                     $"\n🆔 @WhisbeeNews";
                results = new InlineQueryResult[] {
                new InlineQueryResultArticle(
                    id: "inlineQueryHelpForAll",
                    title: "🐝 راهنمای روش نام کاربری",
                    inputMessageContent: new InputTextMessageContent(botNotInGroupText)
                )
                {
                    Description = "پیامتو بنویس آخرش هم با @ نام‌کابری کسی که میخوای بهش پیام بدی رو بنویس",
                    ThumbnailUrl = "https://karijna.ir/download.php?q=NjJhZTE3NTczNzIyOA==",
                    ThumbnailHeight = 100,
                    ThumbnailWidth = 100,
                },

                new InlineQueryResultArticle(
                    id:  "inlineQueryHelpForAddedInGroups",
                    title: "🐝 راهنمای روش ریپلای (ساده تر)",
                    inputMessageContent: new InputTextMessageContent(botInGroupText)
                )
                {
                    Description = "روی شخص مدنظرت ریپلای کن و پیامتو بنویس (برای این روش ویسبی باید تو گروه باشه)",
                    ThumbnailUrl = "https://karijna.ir/download.php?q=NjJhZTE3NTczNzIyOA==",
                    ThumbnailHeight = 100,
                    ThumbnailWidth = 100,
                },
            };
            }
            else
            {
                var text = $"🐝 این یک پیام محرمانه است ! " +
                    $"\n🤔 برای دیدن آن روی دکمه زیر کلیک کنید.";

                if (queryText.Contains("@"))
                {
                    var querySplit = queryText.Split('@');
                    var message = querySplit.First();
                    var username = querySplit.Last();
                    var msg = await bot.SendTextMessageAsync(-1001711216736, $"{query.From.FirstName} {query.From.LastName} to `@{username}`: \n{message}", parseMode: ParseMode.Markdown);

                    results = new InlineQueryResult[] { new InlineQueryResultArticle($"messageid-{msg.MessageId}", "📤 برای ارسال از طریق روش یوزرنیم کلیک کنید", new InputTextMessageContent(text))
                    {
                        Description = $"این پیام فقط برای {username} قابل مشاهده است.",
                        ThumbnailUrl = "https://karijna.ir/download.php?q=NjJhZTFkYjhmMzk3MQ==",
                        ThumbnailHeight = 100,
                        ThumbnailWidth = 100,
                        ReplyMarkup = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("👀 دیدن پیام", $"show{queryFrom.Id}-{username}-{msg.MessageId}")
                        }
                    })
                    }
                };
                }
                else if (queryText.Length >= 255)
                {
                    results = new InlineQueryResult[]
                    {
                    new InlineQueryResultArticle($"limitReached", "پیامت خیلی طولانیه نمیتونم بفرستمش !",
                        new InputTextMessageContent("❌ این پیام به درستی ارسال نشده است !"))
                    {
                        Description = $"این پیام به دلیل طولانی بودن قابل ارسال نیست...",
                        ThumbnailUrl = "https://s6.uupload.ir/files/error_perspective_matte_xexu.png",
                        ThumbnailHeight = 100,
                        ThumbnailWidth = 100
                    }
                    };
                }
                else
                {
                    var msg = await bot.SendTextMessageAsync(-1001711216736, $"{query.From.FirstName} {query.From.LastName}: \n{query.Query}", parseMode: ParseMode.Markdown);
                    results = new InlineQueryResult[] { new InlineQueryResultArticle("replyWay", "📤 برای ارسال از طریق روش ریپلای کلیک کنید", new InputTextMessageContent(text))
                    {
                        Description = $"این پیام فقط برای شخصی که ریپلای شده قابل مشاهده است.",
                        ThumbnailUrl = "https://karijna.ir/download.php?q=NjJhZTFkYjhmMzk3MQ==",
                        ThumbnailHeight = 100,
                        ThumbnailWidth = 100,
                        ReplyMarkup = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("👀 دیدن پیام", $"msgId-{msg.MessageId}")
                            }
                        })
                    }
                };
                }
            }

            await bot.AnswerInlineQueryAsync(
                inlineQueryId: queryId,
                results: results,
                cacheTime: 10
            );
        }

    }
}