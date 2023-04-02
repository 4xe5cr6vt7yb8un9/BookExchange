using CsvHelper;
using HtmlAgilityPack;
using System.Globalization;
using ScrapySharp.Extensions;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace BookExchange
{
    public class ISBNScraper
    {
        public static Book Scrap(String ISBN)
        {
            //Send the request to the server
            ISBN = ISBN.Replace(" ", String.Empty).Replace("-", String.Empty);

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
                String imageUrl = item.GetAttributeValue("src");
                WebClient webClient = new WebClient();
                webClient.DownloadFile(imageUrl, "C:\\Users\\aiden\\source\\repos\\BookExchange\\Data\\ICONS\\"+ISBN+".jpg");
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
                        String author = item.LastChild.InnerText.Replace("\"", String.Empty).Trim();
                        newBook.Author = author;
                        break;
                    case "Published:":
                        String published = item.LastChild.InnerText.Replace("\"", String.Empty).Trim();
                        newBook.Published = published;
                        break;
                }
            }
            return newBook;
        }

    }
}
