using Google.Apis.Books.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookExchange.Models
{
    public class Book
    {
        //public int Id { get; set; }
        public Guid BookID { get; set; }

        [Required(ErrorMessage = "Book Title is Required")]
        [Display(Name = "Book Title")]
        public String Title { get; set; }

        [Required(ErrorMessage = "Book Author is Required")]
        [Display(Name = "Authors")]
        public String Author { get; set; }

        public String? Description { get; set; }

        public String? Published { get; set; }

        [Required(ErrorMessage = "Book ISBN is Required")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters")]
        [RegularExpression(@"^[+]?[0-9]+$", ErrorMessage = "Only Numbers are allowed")]
        [Display(Name = "ISBN")]
        public String ISBN { get; set; }

        public int Available { get; set; }
    }

    public class ClassUsed
    {
        //public int Id { get; set; }
        public Guid ClassUsedID { get; set; }
        public String className { get; set; }
        public String ISBN { get; set; }
    }

    public class ISBNData
    {
        public String Title { get; set; }
        public String? Subtitle { get; set; }
        public IList<String> Authors { get; set; }
        public String? Published_date { get; set; }
        public String? Description { get; set; }
        public IList<Volume.VolumeInfoData.IndustryIdentifiersData> IndustryIdentifiers { get; set; }
        public Volume.VolumeInfoData.ImageLinksData? ImageLinks { get; set; }
    }
}