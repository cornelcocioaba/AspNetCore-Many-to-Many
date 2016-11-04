using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManyToMany.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string CustomerDisplayName { get; set; }

        public List<CustomerDevice> CustomerDevices { get; set; }
    }
}
