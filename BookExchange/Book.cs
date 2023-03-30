namespace BookExchange
{
    public class Book
    {
        public String Name { get; set; }

        public String Author { get; set; }

        public String Description { get; set; }

        public DateTime ReleasedDate { get; set; } 

        public String ISBN { get; set; }

        public int available { get; set; }

        public List<ExchangeUser> loaners { get; set; }

        public List<ExchangeUser> borrowers { get; set; }
    }
}