using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookExchange.Models
{
    public class Classes
    {
        [Key]
        public Guid ClassID { get; set; }

        [Display(Name="Class Name")]
        [Required(ErrorMessage = "Class Name is required")]
        public string Name { get; set; }

        [Display(Name = "Class Grade")]
        [Required(ErrorMessage = "Class Grade is required")]
        public int Grade { get; set; }

        [Display(Name = "Class Teacher")]
        [Required(ErrorMessage = "Class Teacher is required")]
        public string Teacher { get; set; }
    }

    public class ClassBook
    {
        [Key]
        public Guid id { get; set; }

        public Guid ClassID { get; set; }

        public Guid BookID { get; set; }
    }
}
