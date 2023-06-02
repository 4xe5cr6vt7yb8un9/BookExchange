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
using Microsoft.Extensions.Hosting.Internal;
using System;
using Google.Apis.Books.v1.Data;
using System.Linq;
using System.Text;

namespace BookExchange.Actions
{
    public static class ISBNScraper
    {
        private static readonly String apiKey = "AIzaSyCl83uxcTL1Zg4p_mxUajhJhEHsjgUy94k";

        public async static Task DownloadImage(String URL, String ISBN)
        {
            Uri uri = new(URL);
            HttpClient client = new();
            var response = await client.GetAsync(uri);
            try
            {
                using var fs = new FileStream(
                                string.Format("wwwroot/data/thumbnails/{0}.jpg", ISBN),
                                FileMode.CreateNew);
                await response.Content.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to Save File");
                Console.WriteLine("Unable to Save File: " + ex.Message);
            }
        }

        public static Boolean bookExists(String ISBN)
        {
            try
            {
                BookApi GBooks = new(apiKey);
                List<ISBNData> results = GBooks.Search("ISBN:" + ISBN);
                ISBNData? correctBook = null;

                Book newBook = new();

                IEnumerable<ISBNData> bookQuery =
                    from book in results
                    from identity in book.IndustryIdentifiers
                    where identity.Identifier.Equals(ISBN)
                    select book;

                foreach (var book in bookQuery)
                {
                    correctBook = book;
                }

                Debug.WriteLine(correctBook != null);
                return correctBook != null;
            }
            catch (Exception) {
                Debug.WriteLine("Error while Verifing Book");
                Console.WriteLine("Error while Verifing Book");
                return false;
            }
        }

        public static async Task<Book?> ISBNGrabAsync(String ISBN)
        {
            try
            {
                BookApi GBooks = new(apiKey);
                List<ISBNData> results = GBooks.Search("ISBN:" + ISBN);
                ISBNData? correctBook = null;

                Book newBook = new();

                IEnumerable<ISBNData> bookQuery =
                    from book in results
                    from identity in book.IndustryIdentifiers
                    where identity.Identifier.Equals(ISBN)
                    select book;

                foreach (var book in bookQuery)
                {
                    correctBook = book;
                }

                if (correctBook != null)
                {
                    StringBuilder authors = new("");

                    foreach (var author in correctBook.Authors.ToArray())
                    {
                        authors.Append(author + ", ");
                    }

                    newBook.ISBN = ISBN;
                    newBook.Author = authors.ToString();
                    newBook.Title = correctBook.Title;
                    newBook.Published = correctBook.Published_date;
                    newBook.Description = correctBook.Description;

                    if (correctBook.ImageLinks != null)
                        await DownloadImage(correctBook.ImageLinks.Thumbnail, ISBN);

                    return newBook;
                }

                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to find book: " + ex.Message);
                Console.WriteLine("Unable to find book: " + ex.Message);
                return null;
            }
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
