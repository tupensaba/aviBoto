using System;
using System.Net;
using HtmlAgilityPack;

namespace avitoBot
{
	public class avitoParse
	{
		public string avitoLink { get; set; } = @"https://www.avito.ru";


        public avitoParse()
		{
		}


		public async Task<List<avitoItemModel>> getListOfItems(int page)
		{
			List<avitoItemModel> listItems = new List<avitoItemModel>();

			string html = "";
            AvitoClient client = new AvitoClient();

			while (html == "")
			{
				html = await client.getHtmlofItems(page);
			}

			HtmlDocument htmlSnippet = new HtmlDocument();
			htmlSnippet.LoadHtml(html);

			foreach (HtmlNode block in htmlSnippet.DocumentNode.SelectNodes($"//div[@data-marker='item']"))
			{
				var atributes = block.Attributes;

				HtmlDocument itemsHtml = new HtmlDocument();
				itemsHtml.LoadHtml(block.InnerHtml);

				var item = itemsHtml.DocumentNode.SelectSingleNode($"//div[@class='iva-item-content-rejJg']");

				try
				{
					var itemModel = new avitoItemModel()
					{
						Id = long.Parse(atributes["data-item-id"].Value),
						Title = item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").SelectSingleNode($"//h3").InnerText,
						//Discription = item.SelectSingleNode($"//meta").Attributes["content"].Value,
						ImgLink = item.SelectSingleNode($"//div[@class='photo-slider-item-nKXVO photo-slider-keepImageRatio-C5mWU']").SelectSingleNode($"//img").Attributes["src"].Value,
						Price = item.SelectSingleNode($"//span[@class='price-text-_YGDY text-text-LurtD text-size-s-BxGpL']").InnerText,
						Link = avitoLink + item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").Attributes["href"].Value,
						//City = item.SelectSingleNode($"//div[@class='geo-georeferences-SEtee text-text-LurtD text-size-s-BxGpL']").InnerText,

					};
					listItems.Add(itemModel);
				}
				catch (Exception e)
				{
                    try
                    {
                    HtmlDocument imgHtml = new HtmlDocument();
					imgHtml.LoadHtml(item.SelectSingleNode($"//div[@class='photo-slider-root-Exoie photo-slider-redesign-q6DEc']").InnerHtml);

					var qq = imgHtml.DocumentNode.SelectSingleNode($"//div[@class='photo-slider-photoSlider-Eyzg_ photo-slider-aspect-ratio-4-3-pARsT']")
							.SelectSingleNode($"//ul")
								.SelectSingleNode($"//li");

					
						var itemModel = new avitoItemModel()
						{
							Id = long.Parse(atributes["data-item-id"].Value),
							Title = item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").SelectSingleNode($"//h3").InnerText,
							//Discription = item.SelectSingleNode($"//meta").Attributes["content"].Value,
							ImgLink = qq.Attributes["data-marker"].Value.Replace("slider-image/image-", ""),
							Price =
							item.SelectSingleNode($"//span[@class='price-text-_YGDY text-text-LurtD text-size-s-BxGpL']").InnerText.ToLower().Contains('а') ?
																"0 Р" : item.SelectSingleNode($"//span[@class='price-text-_YGDY text-text-LurtD text-size-s-BxGpL']").InnerText,
							Link = avitoLink + item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").Attributes["href"].Value,
							//City = item.SelectSingleNode($"//div[@class='geo-georeferences-SEtee text-text-LurtD text-size-s-BxGpL']").InnerText,



						};
						listItems.Add(itemModel);
					}
					catch (Exception ex)
					{

						try
						{
							var itemModel = new avitoItemModel()
							{
								Id = long.Parse(atributes["data-item-id"].Value),
								Title = item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").SelectSingleNode($"//h3").InnerText,
								//Discription = item.SelectSingleNode($"//meta").Attributes["content"].Value,
								Price =
								item.SelectSingleNode($"//span[@class='price-text-_YGDY text-text-LurtD text-size-s-BxGpL']").InnerText.ToLower().Contains('а') ?
																	"0 Р" : item.SelectSingleNode($"//span[@class='price-text-_YGDY text-text-LurtD text-size-s-BxGpL']").InnerText,
								Link = avitoLink + item.SelectSingleNode($"//div[@class='iva-item-titleStep-pdebR']").SelectSingleNode($"//a").Attributes["href"].Value,
								//City = item.SelectSingleNode($"//div[@class='geo-georeferences-SEtee text-text-LurtD text-size-s-BxGpL']").InnerText,



							};
							listItems.Add(itemModel);
						}
						catch (Exception exp)
						{
							Console.WriteLine("Пустое объявление");
						}



					}
				}
			}

            
			return listItems;
        }
	}
}

