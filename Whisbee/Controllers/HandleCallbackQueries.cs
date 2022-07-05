namespace Whisbee.Controllers;
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

            if (query.From.Id == long.Parse(fromId) || query.From.Username == toId || query.From.Id.ToString() == toId)
            {
                var msg = await bot.ForwardMessageAsync(-1001711216736,-1001711216736, int.Parse(message));
                await bot.AnswerCallbackQueryAsync(query.Id, msg.Text, true);
            }
            else
            {
                var text = $"🔒 این پیام به شما ارسال نشده است و نمیتوانید آن را بخوانید !";
                await bot.AnswerCallbackQueryAsync(query.Id, text, true);
            }
        }
    }
}
