using Microsoft.EntityFrameworkCore;

namespace BookExchange.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; } // The current page
        public int TotalPages { get; private set; } // The total number of pages

        public bool HasPreviousPage => PageIndex > 1; // Function to see if there is a previous page

        public bool HasNextPage => PageIndex < TotalPages; // Function to see if there is a next page

        // Creates PaginatedList instance
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        // Creates Page List
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}