using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace avitoBot
{
    public class Program
    {
        public static TelegramBotClient bot { get; set; } = new TelegramBotClient(Configuration.BotToken);

        public static string BotName { get; set; } = "";

        public static List<long> noRepeatList { get; set; } = new List<long>();

        public static string Status { get; set; } = "Выключен";

        public static int Count { get; set; } = 100;

        public static async Task Main()
        {
            if (Configuration.BotToken != "")
            {
                var me = await bot.GetMeAsync();

                me.CanReadAllGroupMessages = true;
                Console.Title = me.Username;

                BotName = me.Username;



                bot.OnMessage += BotOnMessageResultRecieved;
                bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
                bot.OnReceiveError += BotOnReceiveError;

                bot.StartReceiving(Array.Empty<UpdateType>());
                Console.WriteLine($"Start listening for @{me.Username},{me.Id.ToString()}");

                Console.ReadLine();
                bot.StopReceiving();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удается подключиться к боту, проверьте файл keybot.txt на наличие api ключа");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }

        }

        private static async void BotOnMessageResultRecieved(object sender, MessageEventArgs message)
        {
            switch(message.Message.Text)
            {
                case "/Включить":

                    Status = "Включен";

                    while (Status == "Включен")
                    {
                        if (Count >= 1)
                        {
                            ThreadStart ts = new ThreadStart(async () =>
                            {

                                avitoParse avitoParse = new avitoParse();

                                var listItems = await avitoParse.getListOfItems(page: Count--);

                                FilterUtils fu = new FilterUtils();

                                var goodItem = await fu.getFilteredListByStopWords(listItems);

                                if (noRepeatList != null && noRepeatList.Any())
                                    goodItem = goodItem.Where(x => !noRepeatList.Contains(x.Id)).ToList();

                                foreach (var good in goodItem)
                                {
                                    if (noRepeatList == null || !noRepeatList.Contains(good.Id))
                                        noRepeatList.Add(good.Id);

                                    await bot.SendPhotoAsync(message.Message.Chat.Id, photo: good.ImgLink, caption: await fu.prepareRenderText(good), parseMode: ParseMode.Html);

                                }

                            });
                            Thread thread = new Thread(ts);
                            thread.Start();

                            Thread.Sleep(5000);
                        }
                        avitoParse avitoParse = new avitoParse();

                        var listItems = await avitoParse.getListOfItems(1);

                        FilterUtils fu = new FilterUtils();

                        var goodItem = await fu.getFilteredListByStopWords(listItems);

                        if (noRepeatList != null && noRepeatList.Any())
                            goodItem = goodItem.Where(x => !noRepeatList.Contains(x.Id)).ToList();

                        foreach (var good in goodItem)
                        {
                            if (noRepeatList == null || !noRepeatList.Contains(good.Id))
                                noRepeatList.Add(good.Id);

                            await bot.SendPhotoAsync(message.Message.Chat.Id, photo: good.ImgLink, caption: await fu.prepareRenderText(good), parseMode: ParseMode.Html);

                        }

                    }

                    break;
            }
           
        }
           


        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Реплай {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");

        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

        public static async Task SendCommands(Message message, string text)
        {

        }

        public static async Task SendMainKeyboard(Message message)
        {
            
        }

    }
}
