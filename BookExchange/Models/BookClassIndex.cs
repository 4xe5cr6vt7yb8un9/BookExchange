namespace BookExchange.Models
{
    public class BookClassIndex
    {
        public PaginatedList<Book> Books { get; set;}
        public IEnumerable<Classes> Classes { get; set;}
    }

}
