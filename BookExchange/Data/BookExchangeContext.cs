using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public DbSet<BookExchange.Models.ExchangeUser>? ExchangeUser { get; set; }
    }
}
