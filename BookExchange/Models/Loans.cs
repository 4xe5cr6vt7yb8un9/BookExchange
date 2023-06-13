using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookExchange.Models
{
    public class Loans
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String LoanerName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String LoanerEmail { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public String LoanerPhone { get; set; }

        public Guid BookID { get; set; }

        [Required]
        [NotMapped]
        [StringLength(13, ErrorMessage = "ISBN must be less than 13 characters long")]
        [Display(Name = "Book ISBN")]
        public String ISBN { get; set; }

        [NotMapped]
        public String title { get; set; }

        [Display(Name = "Date Donated")]
        public DateTime LoanDate { get; set; }
    }
}
