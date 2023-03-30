namespace BookExchange
{
    public class ExchangeUser
    {
        public String Name { get; set; }

        public String email { get; set; }

        public List<String> borrowed { get; set; }

        public List<String> loaned { get; set; }

        public List<String> available { get; set; }
    }
}
