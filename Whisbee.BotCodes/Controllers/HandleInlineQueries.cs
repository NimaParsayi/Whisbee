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
                    title: "🐝 راهنمای روش اول",
                    inputMessageContent: new InputTextMessageContent(botNotInGroupText)
                )
                {
                    Description = "پیام خود را بنویسید و در انتها یوزرنیم شخص مدنظر را بنویسید سپس ارسال کنید...",
                    ThumbUrl = "https://karijna.ir/download.php?q=NjJhZTE3NTczNzIyOA==",
                    ThumbHeight = 100,
                    ThumbWidth = 100,
                },

                new InlineQueryResultArticle(
                    id:  "inlineQueryHelpForAddedInGroups",
                    title: "🐝 راهنمای روش دوم (ساده تر)",
                    inputMessageContent: new InputTextMessageContent(botInGroupText)
                )
                {
                    Description = "ویسبی را به گروه اضافه کنید و روی شخص مدنظر ریپلای کنید سپس پیام خود را ارسال کنید...",
                    ThumbUrl = "https://karijna.ir/download.php?q=NjJhZTE3NTczNzIyOA==",
                    ThumbHeight = 100,
                    ThumbWidth = 100,
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
                    var msg = await bot.SendTextMessageAsync(-1001711216736, $"{query.From.FirstName} {query.From.LastName} to `@{username}`: \n{message}", ParseMode.Markdown);

                    results = new InlineQueryResult[] { new InlineQueryResultArticle($"messageid-{msg.MessageId}", "📤 برای ارسال از طریق روش یوزرنیم کلیک کنید", new InputTextMessageContent(text))
                    {
                        Description = $"این پیام فقط برای {username} قابل مشاهده است.",
                        ThumbUrl = "https://karijna.ir/download.php?q=NjJhZTFkYjhmMzk3MQ==",
                        ThumbHeight = 100,
                        ThumbWidth = 100,
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
                    new InlineQueryResultArticle($"limitReached", "پیام شما بیش از حد طولانیست !",
                        new InputTextMessageContent("❌ این پیام به درستی ارسال نشده است !"))
                    {
                        Description = $"این پیام به دلیل طولانی بودن قابل ارسال نیست...",
                        ThumbUrl = "https://s6.uupload.ir/files/error_perspective_matte_xexu.png",
                        ThumbHeight = 100,
                        ThumbWidth = 100
                    }
                    };
                }
                else
                {
                    var msg = await bot.SendTextMessageAsync(-1001711216736, $"{query.From.FirstName} {query.From.LastName}: \n{query.Query}", ParseMode.Markdown);
                    results = new InlineQueryResult[] { new InlineQueryResultArticle("replyWay", "📤 برای ارسال از طریق روش ریپلای کلیک کنید", new InputTextMessageContent(text))
                    {
                        Description = $"این پیام فقط برای شخصی که ریپلای شده قابل مشاهده است.",
                        ThumbUrl = "https://karijna.ir/download.php?q=NjJhZTFkYjhmMzk3MQ==",
                        ThumbHeight = 100,
                        ThumbWidth = 100,
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