using System;
using System.Configuration;
using System.IO;

namespace avitoBot
{
    public static class Configuration
    {

        public static string BotToken
        {
            get
            {
                try
                {

                    var apikey = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "keybot.txt")).Trim();

                    Console.WriteLine(apikey);

                    return apikey;

                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Не найден keybot.txt\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    return "";

                }
            }
        }

        public static long BotChannel
        {
            get
            {

                try
                {

                    var channelid = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "channel.txt")).Trim();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("ID канала:" + channelid);
                    Console.ForegroundColor = ConsoleColor.White;

                    return Convert.ToInt64(channelid);
                }
                catch (Exception e)
                {



                    return 0;

                }
            }
        }

    }
}

