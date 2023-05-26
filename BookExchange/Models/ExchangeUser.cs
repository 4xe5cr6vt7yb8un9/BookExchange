using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
        [StringLength(13, ErrorMessage = "ISBN must be 13 characters long")]
        [Display(Name = "Book ISBN")]
        public String ISBN { get; set; }

        [Display(Name = "Date Donated")]
        public DateTime LoanDate { get; set; }
    }
    public class Rents
    {
        public Guid Id { get; set; }

        [Display(Name = "Renter Name")]
        public String RenterName { get; set; }

        [Display(Name = "Renter Email")]
        public String RenterEmail { get; set; }

        [Display(Name = "Rented From")]
        public String RentedFrom { get; set; }

        public String ISBN { get; set; }

        [Display(Name = "Rented Date")]
        public DateTime RentDate { get; set; }

        public void Print()
        {
            Console.WriteLine("ID: " + Id);
            Console.WriteLine("RName: " + RenterName);
            Console.WriteLine("REmail: " + RenterEmail);
            Console.WriteLine("RFrom: " + RentedFrom);
            Console.WriteLine("ISBN: " + ISBN);
            Console.WriteLine("RDate: " + RentDate);
        }
    }
}
