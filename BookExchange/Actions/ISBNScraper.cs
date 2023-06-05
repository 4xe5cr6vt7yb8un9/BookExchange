using BookExchange.Models;
using Google.Apis.Books.v1;
using Google.Apis.Services;
using System.Diagnostics;
using System.Text;

namespace BookExchange.Actions
{
    public static class ISBNScraper
    {
        // Google API key for ISBN lookup
        private static readonly String apiKey = "AIzaSyCl83uxcTL1Zg4p_mxUajhJhEHsjgUy94k";

        /// <summary>
        /// Function to download book cover.
        /// </summary>
        /// <param name="ISBN">The ISBN number of book</param>
        /// <param name="URL">URL of book cover to download</param>
        public async static Task DownloadImage(String URL, String ISBN)
        {
            HttpClient client = new(); // Creates HttpClient to get image
            var response = await client.GetAsync(new Uri(URL));

            try
            {
                // Creates new file and writes image content
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

        /// <summary>
        /// Queries google books database
        /// </summary>
        /// <param name="ISBN">ISBN number of the book to find</param>
        /// <returns>Returns Book Information</returns>
        public static ISBNData? queryDatabase(String ISBN)
        {
            try
            {
                BookApi GBooks = new(apiKey); // Creates a book api instance with given key
                List<ISBNData> results = GBooks.Search("ISBN:" + ISBN); // Queries book database with given ISBN

                // Queries results to remove books that dont match given ISBN
                IEnumerable<ISBNData> bookQuery =
                    from book in results
                    where book != null
                    from identity in book.IndustryIdentifiers
                    where identity.Identifier.Equals(ISBN)
                    select book;

                // Returns book info if found else returns null
                if (bookQuery.Any())
                    return bookQuery.First();
                else
                    return null;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while querying Book");
                return null;
            }
        }

        /// <summary>
        /// Verifies if a book with the given ISBN number exists in the google books database
        /// </summary>
        /// <param name="ISBN">The ISBN number to search for book</param>
        /// <returns>Returns true if book is found</returns>
        public static Boolean bookExists(String ISBN)
        {
            try
            {
                // Queries Database for book
                ISBNData? results = queryDatabase(ISBN);

                // If book exists returns true else false
                if (results != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                Console.WriteLine("Error while Verifing Book");
                return false;
            }
        }

        /// <summary>
        /// Searches Google database for book information and casts it to Book model
        /// </summary>
        /// <param name="ISBN">ISBN number of the desired book</param>
        /// <returns>Returns a book model containing relavent information</returns>
        public static async Task<Book?> ISBNGrabAsync(String ISBN)
        {
            try
            {
                // Queries database for book information
                ISBNData? correctBook = queryDatabase(ISBN);

                // Verifies the book was found
                if (correctBook != null)
                {
                    Book newBook = new();
                    StringBuilder authors = new("");

                    // Convert Author list into a single string
                    foreach (var author in correctBook.Authors.ToArray())
                    {
                        authors.Append(author + ", ");
                    }

                    // Casts collected information to book model
                    newBook.ISBN = ISBN;
                    newBook.Author = authors.ToString();
                    newBook.Title = correctBook.Title;
                    newBook.Published = correctBook.Published_date;
                    newBook.Description = correctBook.Description;

                    // If book cover image exists, initiates download
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

    /// <summary>
    /// BookAPI Class to make connection to Google Books Service
    /// </summary>
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

        /// <summary>
        /// Searches Google Database for the first 40 results
        /// </summary>
        /// <param name="query">Query to search by</param>
        /// <returns>Returns a ISBNData list</returns>
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
