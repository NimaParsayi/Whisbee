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
                        msg.Text = $"🔒 این یک پیام محرمانه به *{msg.ReplyToMessage.From.FirstName}* از طرف *{msg.From.FirstName}* است که فقط برای این دو شخص قابل مشاهده است !" +
                                   $"\n\n🐝 ویسبی؛ ساده ترین ربات ارسال پیام محرمانه در تلگرام";
                        var data = msg.ReplyMarkup.InlineKeyboard.ToArray()[0].ToArray()[0].CallbackData;
                        if (data.StartsWith("msgId"))
                        {
                            data = data.Replace("msgId-", null);
                            msg.ReplyMarkup = new InlineKeyboardMarkup(new[]
                                                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("👀 دیدن پیام",
                                $"show{msg.From.Id}-{msg.ReplyToMessage.From.Id}-{data}")
                        }
                        });
                            await bot.EditMessageTextAsync(msg.Chat.Id, msg.MessageId, msg.Text, ParseMode.Markdown, replyMarkup: msg.ReplyMarkup);
                        }
                    }
                }
                else if (msg.Chat.Type is ChatType.Private)
                {
                    var message = $"🐝 با ویسبی میتونی به راحتی توی گروه ها به شخصی که دوست داری پیام محرمانه ارسال کنی !" +
                                  $"\n\n1️⃣ *روش اول:* اول @WhisbeeBot را بنویسید سپس پیام خود و در انتها یوزرنیم کاربر مورد نظر را تایپ کنید و روی پاپ‌آپ باز شده کلیک کنید !" +
                                  $"\n@WhisbeeBot پیام تست @Username" +
                                  $"\n\n2️⃣ *روش دوم (پشنهادی):* برای اینکار کافیست ویسبی را به گروه خود اضافه کنید (نیازی نیست ادمین باشد) سپس با ریپلای رو کاربر مدنظر و نوشتن @Whisbee و پیام خود با کلیک روی پاپ‌آپ باز شده پیام خود را به صورت محرمانه ارسال کنید !" +
                                  $"\n\n⚠️ *ویسبی هیچ نوع دسترسی به پیام های شما ندارد و آن ها را ذخیره نمیکند و کاملا امن است.*";
                    var keyboard = new InlineKeyboardMarkup(new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithUrl("➕ افزودن ویسبی به گروه","http://t.me/whisbeebot?startgroup=new")
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

                    await bot.SendTextMessageAsync(msg.Chat.Id, message, ParseMode.Markdown, replyMarkup: keyboard);
                }
            }
        }
    }
}