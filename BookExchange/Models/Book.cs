using Google.Apis.Books.v1.Data;
using Newtonsoft.Json.Linq;

namespace BookExchange.Models
{
    public class Book
    {
        public String Title { get; set; }

        public String Author { get; set; }

        public String Description { get; set; }

        public String Published { get; set; } 

        public String ISBN { get; set; }

        public int Available { get; set; }

    }

    public class ISBNData
    {
        public String Title { get; set; }
        public String Subtitle { get; set; }
        public IList<String> Authors { get; set; }
        public String Published_date { get; set; }
        public String Description { get; set; }
        public IList<Volume.VolumeInfoData.IndustryIdentifiersData> IndustryIdentifiers { get; set; }
        public Volume.VolumeInfoData.ImageLinksData ImageLinks { get; set; }

    }
}