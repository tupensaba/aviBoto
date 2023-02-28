using System;
namespace avitoBot
{
	public class FilterUtils
	{
		public List<string> StopWord
        {
            get
            {
                try
                {

                    var _stop = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "stop.txt")).Split(',');

                    Console.WriteLine(_stop);

                    return _stop.ToList();

                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Фильтрация без стоп слов");
                    Console.ForegroundColor = ConsoleColor.White;
                    return new List<string>() { };

                }

            }
        }

		public FilterUtils()
		{
		}

        public async Task<List<avitoItemModel>> getFilteredListByStopWords(List<avitoItemModel> listAItem)
        {
            var listIdForDel = new List<long>();
            if(!StopWord.Any())
            {
                return listAItem;
            }

            foreach (var aItem in listAItem)
            {
                foreach (var word in StopWord)
                {
                    if(string.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }
                    if (aItem.Title.ToLower().Contains(word.ToLower().Trim()))
                    {
                        listIdForDel.Add(aItem.Id);
                        continue;
                    }
                }
            }

            listAItem = listAItem.Where(x => !listIdForDel.Contains(x.Id)).ToList();

            return listAItem;
        }

        public async Task<string> prepareRenderText(avitoItemModel AIM)
        {
            var renderedText = $"{AIM.Title} \n";

            renderedText += $"\nЦена: {AIM.Price} \n";

            renderedText += $"\nОписание:{AIM.Discription}\n";

            renderedText += $"\n<a href=\"{AIM.Link}\">ССЫЛКА</a>";

            return renderedText;
        }

	}
}

