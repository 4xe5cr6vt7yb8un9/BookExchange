namespace BookExchange.Models
{
    public class ExchangeUser
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }
    }

    public class Loans
    {
        public Guid Id { get; set; }
        public String ISBN { get; set; }
    }
    public class Borrowed
    {
        public Guid Id { get; set; }
        public String ISBN { get; set; }
    }
}
