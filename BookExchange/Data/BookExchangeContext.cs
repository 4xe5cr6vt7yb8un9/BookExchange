using Microsoft.EntityFrameworkCore;
using BookExchange.Models;

namespace BookExchange.Data
{
    public class BookExchangeContext : DbContext
    {
        public BookExchangeContext (DbContextOptions<BookExchangeContext> options)
            : base(options)
        {
        }

        public DbSet<BookExchange.Models.Book> Book { get; set; } = default!;

        public DbSet<BookExchange.Models.Loans>? Loans { get; set; }

        public DbSet<BookExchange.Models.Classes>? Classes { get; set; }

        public DbSet<BookExchange.Models.ClassBook>? ClassBook { get; set; }
    }
}
