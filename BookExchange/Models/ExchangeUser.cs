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

        [Display(Name = "Loaner Name")]
        public String LoanerName { get; set; }

        [Display(Name = "Loaner Email")]
        public String LoanerEmail { get; set; }

        public String ISBN { get; set; }

        [Display(Name = "Loaned Date")]
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
    }
}
