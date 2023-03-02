using System;
using System.Net;

namespace avitoBot
{
	public class AvitoClient
	{
		public AvitoClient()
		{
		}

		public async Task<string> getHtmlofItems(int page)
		{
            var clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            var client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromSeconds(30);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.avito.ru/sankt_peterburg_i_lo/mebel_i_interer?f=ASgBAgICAUSKvQ280jQ&geoCoords=59.939095%2C30.315868&presentationType=serp&s=104&p={page}"),
                Headers =
                        {
                            { "Cookie", "buyer_from_page=catalog; v=1677511773; _ga=GA1.1.726266468.1677511776; _ga_M29JC28873=GS1.1.1677511775.1.1.1677512909.60.0.0; cto_bundle=nfUFZ19jN/E9Sa0tZYUR2MHBjVGFmSDRlc2hyaDlEdnlyJTJCOUY0N1JHa3UxZ3VQekN6cngyS2djU2tUZ2NGM0tVc3phd0xsdGwzRHRHJTJGbDNlSG5UVm8xRyUyQlJtUDU3VEkxJTJGaXZxYTQ5SnJzTHJGZWJhbWVFUFVLTU1tYkg3a1NodGJ0OFg1; tmr_lvid=4e4392990ff2d11402d37f057b1666fb; tmr_lvidTS=1676294075578; _buzz_fpc=JTdCJTIycGF0aCUyMiUzQSUyMiUyRiUyMiUyQyUyMmRvbWFpbiUyMiUzQSUyMi53d3cuYXZpdG8ucnUlMjIlMkMlMjJleHBpcmVzJTIyJTNBJTIyVHVlJTJDJTIwMjclMjBGZWIlMjAyMDI0JTIwMTUlM0E0OCUzQTI5JTIwR01UJTIyJTJDJTIyU2FtZVNpdGUlMjIlM0ElMjJMYXglMjIlMkMlMjJ2YWx1ZSUyMiUzQSUyMiU3QiU1QyUyMnZhbHVlJTVDJTIyJTNBJTVDJTIyMTBhN2M3ZjUxNjNhOGVjZjlkMjA3OTRjN2JmYjQ0MDElNUMlMjIlMkMlNUMlMjJmcGpzRm9ybWF0JTVDJTIyJTNBdHJ1ZSU3RCUyMiU3RA==; sx=H4sIAAAAAAAC%2F5zMQW6DQAwAwL%2F4zGHXxrs2v7ENJEBEEbQRacTfe6rUcz8wb7AarGhK3qYIQu9Z0UlDTBR1hO4NT%2BhgSp9z%2F7ov5w2%2F9lzldW4fa57XCc993w5oYIAul1pZlZJcDXjk7EHmLFR6TlWjhA81GZKMqf5HTqW9GhiYxCNEk6mXnimyGHvwYCO2RL%2ByP564Ln7Md9PvY%2BN2sanmR%2B3LLQ27%2F5WFUa%2FrJwAA%2F%2F%2B4TW5pDwEAAA%3D%3D; abp=0; tmr_detect=0%7C1677512668742; _ym_visorc=b; SEARCH_HISTORY_IDS=4; buyer_location_id=107621; f=5.d908952b446ec0db4b5abdd419952845a68643d4d8df96e9a68643d4d8df96e9a68643d4d8df96e9a68643d4d8df96e94f9572e6986d0c624f9572e6986d0c624f9572e6986d0c62ba029cd346349f36c1e8912fd5a48d02c1e8912fd5a48d0246b8ae4e81acb9fa143114829cf33ca746b8ae4e81acb9fad99271d186dc1cd062a5120551ae78edaf305aadb1df8ceb48bdd0f4e425aba7085d5b6a45ae867377bdb88c79240d01ff38e8d292af81e50df103df0c26013a2ebf3cb6fd35a0ac71e7cb57bbcb8e0ff0c77052689da50ddc5322845a0cba1aba0ac8037e2b74f92da10fb74cac1eab2da10fb74cac1eab2da10fb74cac1eabdc5322845a0cba1a0df103df0c26013a1d6703cbe432bc2a9302348a0777e4ca2701bf0b8152ebed3ea7d5726ed2b3514c6e394c5c3afe8d02c730c0109b9fbb7b2f02252ec391a44a3d0ad59421601eb9abe6099f96bacbf72316305726b23530c4db039d665204e2415097439d404746b8ae4e81acb9fa786047a80c779d5146b8ae4e81acb9fa319b2fa729ff526d2d38179306cb93212da10fb74cac1eabd1d953d27484fd81b9742f1bd7cdcdbeb46cc5f186872551; " +
                            @"ft=f7KnHENpD2EYriU8NIrGY6w4KCO91pq80tmsFNMmyS85vkFrNKgA3JmSk9UHWkYwNtYvLptCaIrGMe8zEQwLQ6IU0hXPI/IxxtaLoSzfcif5ExsWmgOt0h39QPMFGx/z2BAGLmiicW/aGqPx2OuYS0npn1cM4jIOybjavQmGujrGRXnuwlyvH5mN5R9pQdgH; adrcid=A4lri3KSURRKTszTKAj7imQ; adrdel=1; _gcl_au=1.1.1048464744.1677511775; _ym_d=1677511775; _ym_isad=2; _ym_uid=1676294076347453617; luri=sankt_peterburg_i_lo; redirectMav=1; buyer_laas_location=637640; u=2tkw1189.1fd5ceo.12l7aajsacag0" },
                            { "Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8" },
                            { "Host", "www.avito.ru" },
                            { "User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.0 Safari/605.1.15" },
                            { "Accept-Language", "ru" },
                            { "Referer", $"https://www.avito.ru/sankt_peterburg_i_lo/mebel_i_interer?f=ASgBAgICAUSKvQ280jQ&geoCoords=59.939095%2C30.315868&presentationType=serp&s=104&p={page}" },
                            { "Connection", "keep-alive" },
                        },
            };
            try
            {
                using (var response = await client.SendAsync(request))
            {
                
                    //response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();

                    return body;
               
            }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Bad request");
                return "";
            }
        }

	}
}

