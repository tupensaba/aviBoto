using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace avitoBot
{
    public class Program
    {
        
        public static TelegramBotClient bot { get; set; } = new TelegramBotClient(Configuration.BotToken);

        public static string BotName { get; set; } = "";

        public static List<long> noRepeatList { get; set; } = new List<long>();

        public static string BOTstatus { get; set; } = "Выключен";

        public static int _Pages { get; set; }

        public static int ItemMaxPrice { get; set; } = 0;
        public static int ItemMinPrice { get; set; } = 0;

        public static async Task Main()
        {
            if (Configuration.BotToken != "")
            {
                var me = await bot.GetMeAsync();

                me.CanReadAllGroupMessages = true;
                Console.Title = me.Username;

                BotName = me.Username;



                bot.OnMessage += BotOnMessageResultRecieved;
                bot.OnCallbackQuery += CallbackProgram;
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
            var me = await bot.GetMeAsync();

            message.Message.Text = message.Message.Text.Replace($"@{me.Username}", "").Trim();

            if (message.Message.Text.Contains("!Pages:"))
            {
                try
                {
                    int pages = Convert.ToInt32(message.Message.Text.Replace("!Pages:", "").Trim());

                    _Pages = pages;

                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                     {

                            new []
                            {

                                InlineKeyboardButton.WithCallbackData("Включить?","/parsepages"),
                                
                            },

                      });

                    await bot.SendTextMessageAsync(message.Message.Chat.Id, $"Я собираюсь спарсить {_Pages} страниц. Включите меня!", replyMarkup: inlineKeyboard);

                }
                catch(Exception e)
                {
                    await FilterUtils.setPages(message.Message, bot,"Введено некорректное число. Попробуйте снова");
                }
             }

            if (message.Message.Text.Contains("!setMinPrice:"))
            {
                try
                {
                    int minPrice = Convert.ToInt32(message.Message.Text.Replace("!setMinPrice:", "").Split('\n')[0].Trim());

                    ItemMinPrice = minPrice;
                }
                catch (Exception e)
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {

                    new []
                    {

                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Фильтр Цены:", $"!setMinPrice: {ItemMinPrice}\n" + $"\n!setMaxPrice: {ItemMaxPrice}"),
                        InlineKeyboardButton.WithCallbackData("Меню", "/menu"),

                    },

              });
                    await bot.SendTextMessageAsync(
                        chatId: message.Message.Chat.Id,
                        "Некорректное число у минимальной цены. Попробуйте снова.",
                        replyMarkup: inlineKeyboard

                    );
                }

            }

            if (message.Message.Text.Contains("!setMaxPrice:"))
            {
                try
                {
                    int maxPrice = Convert.ToInt32(message.Message.Text.Replace("!setMaxPrice:", "").Split('\n')[1].Trim());

                    ItemMaxPrice = maxPrice;

                    await bot.SendTextMessageAsync(
                        chatId: message.Message.Chat.Id,
                        $"Минимальная цена: {ItemMinPrice}\n Максимальная цена: {ItemMaxPrice}\n");
                }
                catch (Exception e)
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {

                    new []
                    {

                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Фильтр Цены:", $"!setMinPrice: {ItemMinPrice}\n" + $"\n!setMaxPrice: {ItemMaxPrice}"),
                        InlineKeyboardButton.WithCallbackData("Меню", "/menu"),

                    },
                   
              });
                    await bot.SendTextMessageAsync(
                        chatId: message.Message.Chat.Id,
                        "Некорректное число у максимальной цены. Попробуйте снова.",
                        replyMarkup: inlineKeyboard

                    );
                }
            }

           

            if (message.Message.Text.Contains("!StopWords:"))
            {
                try
                {
                    var stopwords = message.Message.Text.Replace("!StopWords:", "").Split(',');

                    FilterUtils filteru = new FilterUtils();
                    var listofWords = await filteru.addStopWord(stopwords.ToList());

                    var text = "";

                    foreach (var word in listofWords)
                    {
                        text += word + ",";
                    }

                    await bot.SendTextMessageAsync(message.Message.Chat.Id, text != "" ? text : "Список пустой");

                }
                catch (Exception e)
                {
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {

                    new []
                    {

                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Добавить стоп-слово(а)", $"!StopWords: ..."),
                        InlineKeyboardButton.WithCallbackData("Меню", "/menu"),

                    },

              });
                    await bot.SendTextMessageAsync(
                        chatId: message.Message.Chat.Id,
                        "Некорректное запись. Попробуйте снова.",
                        replyMarkup: inlineKeyboard

                    );
                }
            }

            switch (message.Message.Text.ToLower())
            {
                case "/start":
                    await SendMainKeyboard(message.Message);
                    break;
                case "/menu":
                case "/меню":
                    await SendMenu(message.Message, "Меню");
                    break;

                case "/выкл":
                case "/off":
                    BOTstatus = "Выключен";
                    await bot.SendTextMessageAsync(message.Message.Chat.Id, text: BOTstatus);
                    break;
                    
                case "/status":
                case "/статус":

                    await bot.SendTextMessageAsync(message.Message.Chat.Id, text: BOTstatus);

                    break;
                   
            }
           
        }



        public static async void CallbackProgram(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await bot.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);


            switch (callbackQuery.Data.ToLower())
            {
                case "/menu":
                    await SendMenu(callbackQuery.Message, "Меню");
                    break;

                case "/настройка":
                    await FilterUtils.setPages(callbackQuery.Message, bot);
                    break;

                case "/parsepages":

                    var status = $"парисинг {_Pages} от {callbackQuery.Message.Chat.FirstName}";

                    int count = 1;

                    BOTstatus = status;

                    while (BOTstatus == status)
                    {
                        if ( count <= _Pages) // надо поменять для каждого свои страницы 
                        {
                            ThreadStart ts = new ThreadStart(async () =>
                            {
                                avitoParse avitoParse = new avitoParse();
 
                                var listItems = await avitoParse.getListOfItems(page: count++);

                                FilterUtils fu = new FilterUtils();

                                var goodItem = await fu.getFilteredListByStopWords(listItems);

                                goodItem = await fu.filterByPrice(goodItem, ItemMinPrice, ItemMaxPrice);

                                if (noRepeatList != null && noRepeatList.Any())
                                    goodItem = goodItem.Where(x => !noRepeatList.Contains(x.Id)).ToList();

                                foreach (var good in goodItem)
                                {
                                    if (noRepeatList == null || !noRepeatList.Contains(good.Id))
                                        noRepeatList.Add(good.Id);

                                    await bot.SendPhotoAsync(callbackQuery.Message.Chat.Id, photo: good.ImgLink, caption: await fu.prepareRenderText(good), parseMode: ParseMode.Html);

                                }

                            });
                            Thread thread = new Thread(ts);
                            thread.Start();
                            Thread.Sleep(3000);

                        }

                    }

                    break;

                case "/проверкановых":

                    BOTstatus = "Включен";

                    while (BOTstatus == "Включен")
                    {
                        avitoParse avitoParse = new avitoParse();

                        var listItems = await avitoParse.getListOfItems(1);

                        FilterUtils filterUtils = new FilterUtils();

                        var goodItem = await filterUtils.getFilteredListByStopWords(listItems);

                        goodItem = await filterUtils.filterByPrice(goodItem, ItemMinPrice, ItemMaxPrice);

                        if (noRepeatList != null && noRepeatList.Any())
                            goodItem = goodItem.Where(x => !noRepeatList.Contains(x.Id)).ToList();

                        foreach (var good in goodItem)
                        {
                            if (noRepeatList == null || !noRepeatList.Contains(good.Id))
                                noRepeatList.Add(good.Id);

                            await bot.SendPhotoAsync(callbackQuery.Message.Chat.Id, photo: good.ImgLink, caption: await filterUtils.prepareRenderText(good), parseMode: ParseMode.Html);

                        }
                    }
                    break;

                case "/stopwords":

                    FilterUtils fu = new FilterUtils();
                    var text = "";

                    foreach (var word in fu.StopWord)
                    {
                        text += word + ",";
                    }

                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text != "" ? text:"Список пустой");

                    break;

                case "/выкл":
                case "/off":
                    BOTstatus = "Выключен";

                    await bot.SendTextMessageAsync(callbackQuery.Message.Chat.Id, text: $"Статус бота: {BOTstatus}");

                    break;
            }


            callbackQuery.Message.Text = callbackQuery.Data;

        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

        public static async Task SendMenu(Message message, string text)
        { 
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                
                    // first row
                    new []
                    {

                        InlineKeyboardButton.WithCallbackData("Парсинг 1-ой стр:","/проверкановых" ),
                        InlineKeyboardButton.WithCallbackData("Парсинг n-стр:","/настройка"),

                    },
                    new []
                    {
                        
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Фильтр Цены:", $"!setMinPrice: {ItemMinPrice}\n" + $"!setMaxPrice: {ItemMaxPrice}"),
                        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Добавить стоп-слово(а)", $"!StopWords: ..."),
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Вывести стоп-слова", "/stopwords"),

                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Выключить", "/выкл"),

                    }

              });
            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text,
                replyMarkup: inlineKeyboard

            );

        }

        public static async Task SendMainKeyboard(Message message)
        {
            var mainKeyboard = new ReplyKeyboardMarkup(
                new KeyboardButton[][]
                {
                        new KeyboardButton[] { "/menu" },
                        new KeyboardButton[] { "/вкл" },
                       
                }

            );
            mainKeyboard.ResizeKeyboard = true;

            var text = $"Привет {message.Chat.FirstName}!";
            
            await bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text,
                replyMarkup: mainKeyboard

            );
        }

    }
}
