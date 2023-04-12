using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Net;
using BookExchange.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Google.Apis.Books.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookExchange.Actions
{
    public class ISBNScraper
    {
        public static void DownloadImage(String URL, String ISBN)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(URL, "C:\\Users\\aiden\\source\\repos\\BookExchange\\Data\\ICONS\\" + ISBN + ".jpg");
        }
        
        public static Book ISBNGrab(String ISBN)
        {
            BookApi GBooks = new("AIzaSyCl83uxcTL1Zg4p_mxUajhJhEHsjgUy94k");
            List<ISBNData> results = GBooks.Search("ISBN:" + ISBN);
            ISBNData correctBook = new();

            Book newBook = new();
            newBook.ISBN = ISBN;

            foreach (var book in results)
            {
                foreach (var identity in book.IndustryIdentifiers)
                {
                    if (identity.Identifier.Equals(ISBN))
                        correctBook = book;
                }                
            }

            newBook.Author = correctBook.Authors.ToArray()[0];
            newBook.Title = correctBook.Title;
            newBook.Published = correctBook.Published_date;
            newBook.Description = correctBook.Description;

            Console.WriteLine(correctBook.ImageLinks.Thumbnail);
            DownloadImage(correctBook.ImageLinks.Thumbnail, ISBN);

            return newBook;
        }
    }

    public class BookApi
    {
        private readonly BooksService _booksService;
        public BookApi(string apiKey)
        {
            _booksService = new BooksService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = this.GetType().ToString()
            });
        }
        public List<ISBNData> Search(string query)
        {
            var listquery = _booksService.Volumes.List(query);
            listquery.MaxResults = 40;
            listquery.StartIndex = 0;
            var res = listquery.Execute();
            var books = res.Items.Select(b => new ISBNData
            {
                Title = b.VolumeInfo.Title,
                Subtitle = b.VolumeInfo.Subtitle,
                Description = b.VolumeInfo.Description,
                Published_date = b.VolumeInfo.PublishedDate,
                Authors = b.VolumeInfo.Authors,
                IndustryIdentifiers = b.VolumeInfo.IndustryIdentifiers,
                ImageLinks = b.VolumeInfo.ImageLinks
            }).ToList();
            return books;
        }
    }
}
