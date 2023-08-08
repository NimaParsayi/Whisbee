using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Whisbee.Controllers
{

    public class HandleMessages
    {
        public async Task CheckMessage(ITelegramBotClient bot, Message msg)
        {
            if (msg.Type is MessageType.Text)
            {
                if (msg.Chat.Type is ChatType.Supergroup or ChatType.Group)
                {
                    if (msg.Text is "🐝 این یک پیام محرمانه است ! \n🤔 برای دیدن آن روی دکمه زیر کلیک کنید." && msg.ReplyToMessage is not null && msg.ReplyMarkup is not null)
                    {
                        msg.Text =
                            $"🐝 پیام جدید ! *{msg.ReplyToMessage.From.FirstName}*، یه پیام از طرف *{msg.From.FirstName}* داری.";
                        var data = msg.ReplyMarkup.InlineKeyboard.ToArray()[0].ToArray()[0].CallbackData;
                        if (data.StartsWith("msgId"))
                        {
                            data = data.Replace("msgId-", null);
                            msg.ReplyMarkup = new InlineKeyboardMarkup(new[]
                                                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("👀 خوندن پیام",
                                $"show{msg.From.Id}-{msg.ReplyToMessage.From.Id}-{data}"),
                            InlineKeyboardButton.WithUrl("🐝 کانال ویسبی", "https://WhisbeeNews.t.me"),
                        },
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("وضعیت پیام: ❌ خونده نشده", "null"),
                        }
                        });
                            await bot.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, msg.Text, ParseMode.Markdown, replyMarkup: msg.ReplyMarkup);
                        }

                    }
                }
                else if (msg.Chat.Type is ChatType.Private)
                {
                    var message = $"🐝  دیدی یه وقتایی تو گروه دوست داری یه جیزی بگی ولی میخوای فقط یه نفر بخونتش؟ ویسبی اینجا کمکت می‌کنه 😁" +
                                  $"\n\n1️⃣ *روش اول:* اول @WhisbeeBot رو بنویس بعدم پیامتو بنویس تهشم یوزرنیم کسی که میخوای براش پیام بفرستی رو بنویس و روی پاپ‌آپی که باز میشه کلیک کن. ساده بود نه؟! اینم یه نمونه :" +
                                  $"\n@WhisbeeBot پیام تست @Username" +
                                  $"\n\n2️⃣ *روش دوم (پیشنهادی):* منو ببر به گروهی که میخوای (لازم نیست ادمین باشم) بعد رو کسی که مدنظرته ریپلای کن بنویس @Whisbeebot بعد پیامتو بنویس آخرشم روی پاپ‌آپی که باز میشه کلیک کن و تموم. این یکی دیگه واقعا سادست :)" +
                                  $"\n\n🔒 ویسبی همه اطلاعات رو بر بستر تلگرام ذخیره می‌کنه و هیچ پایگاه داده (دیتابیس) ای نداره. پس ما دسترسی ای به پیامای شما نداریم !";
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithUrl("➕ بردن من به گروه","http://t.me/whisbeebot?startgroup=new")
                    },
                    new []
                    {
                        InlineKeyboardButton.WithSwitchInlineQuery("🔒 ارسال پیام محرمانه", ""),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithUrl("🐝 کانال اطلاع رسانی ویسبی","http://t.me/whisbeenews")
                    },
                });

                    await bot.SendTextMessageAsync(msg.Chat.Id, message, parseMode:ParseMode.Markdown, replyMarkup: keyboard);
                }
            }
        }
    }
}