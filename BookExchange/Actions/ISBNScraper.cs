using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Net;
using BookExchange.Models;

namespace BookExchange.Actions
{
    public class ISBNScraper
    {
        public static Book Scrap(string ISBN)
        {
            //Send the request to the server
            ISBN = ISBN.Replace(" ", string.Empty).Replace("-", string.Empty);

            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load("http://api.scraperapi.com?api_key=39a77dc21bd643280e402b3741f4e780&url=https://isbnsearch.org/isbn/" + ISBN);

            //Select a specific node from the HTML doc
            var image = doc.DocumentNode.CssSelect("div.image > img");
            var header = doc.DocumentNode.CssSelect("div.bookinfo > h1");
            var bookinfo = doc.DocumentNode.CssSelect("div.bookinfo > p");

            //Extract the text and store it in a CSV file

            Book newBook = new();
            newBook.ISBN = ISBN;

            foreach (var item in image)
            {
                string imageUrl = item.GetAttributeValue("src");
                WebClient webClient = new WebClient();
                webClient.DownloadFile(imageUrl, "C:\\Users\\aiden\\source\\repos\\BookExchange\\Data\\ICONS\\" + ISBN + ".jpg");
            }

            foreach (var item in header)
            {
                newBook.Title = item.InnerText;
            }

            foreach (var item in bookinfo)
            {
                switch (item.FirstChild.InnerText)
                {
                    case "Author:":
                        string author = item.LastChild.InnerText.Replace("\"", string.Empty).Trim();
                        newBook.Author = author;
                        break;
                    case "Published:":
                        string published = item.LastChild.InnerText.Replace("\"", string.Empty).Trim();
                        newBook.Published = published;
                        break;
                }
            }
            return newBook;
        }

    }
}
