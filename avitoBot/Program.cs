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

                avitoParse avitoParse = new avitoParse();

                await avitoParse.getListOfItems();

                Console.ReadLine();
                bot.StopReceiving();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Не удается подключиться к боту, проверьте файл keybot.txt на наличия api ключа");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
            }

        }

        private static async void BotOnMessageResultRecieved(object sender, MessageEventArgs message)
        {




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
