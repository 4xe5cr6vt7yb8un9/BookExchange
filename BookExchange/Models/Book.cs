using Google.Apis.Books.v1.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookExchange.Models
{
    [Index(nameof(ISBN13), IsUnique = true)]
    [Index(nameof(ISBN10), IsUnique = true)]

    public class Book
    {
        public Guid BookID { get; set; }

        [Required(ErrorMessage = "Book Title is Required")]
        [Display(Name = "Title")]
        public String Title { get; set; }

        [Display(Name = "Subtitle")]
        public String? Subtitle { get; set; }

        [Required(ErrorMessage = "Book Author is Required")]
        [Display(Name = "Authors")]
        public String Author { get; set; }

        public String? Description { get; set; }

        public String? Published { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be 13 characters long")]
        [RegularExpression(@"^[+]?[0-9]+$", ErrorMessage = "Invalid ISBN-13 format")]
        [Display(Name = "ISBN-13")]
        public String ISBN13 { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 characters long")]
        [RegularExpression(@"^[+]?[0-9xX]+$", ErrorMessage = "Invalid ISBN-10 format")]
        [Display(Name = "ISBN-10")]
        public String ISBN10 { get; set; }

        [NotMapped]
        [Display(Name = "Classes")]
        public virtual List<Guid> ClassIds { get; set; }
    }

    public class ISBNData
    {
        public String Title { get; set; }
        public String? Subtitle { get; set; }
        public IList<String?> Authors { get; set; }
        public String? Published_date { get; set; }
        public String? Description { get; set; }
        public IList<Volume.VolumeInfoData.IndustryIdentifiersData> IndustryIdentifiers { get; set; }
        public Volume.VolumeInfoData.ImageLinksData? ImageLinks { get; set; }

        public void Print()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Subtitle: " + Subtitle);
            Console.WriteLine("PublishedDate: " + Published_date);
        }
    }

    public class ISBNdata
    {
        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters long")]
        [RegularExpression(@"^[+]?[0-9xX]+$", ErrorMessage = "Invalid ISBN format")]
        [Display(Name = "ISBN")]
        public String ISBN { get; set; }
    }
}