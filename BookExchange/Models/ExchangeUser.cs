using System.ComponentModel.DataAnnotations;

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

        [Required]
        [Display(Name = "Name")]
        public String LoanerName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public String LoanerEmail { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "ISBN must be less than 13 characters long")]
        [Display(Name = "Book ISBN")]
        public String ISBN { get; set; }

        [Display(Name = "Date Donated")]
        public DateTime LoanDate { get; set; }
    }
}
