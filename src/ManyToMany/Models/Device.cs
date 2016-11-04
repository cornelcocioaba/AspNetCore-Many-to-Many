using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManyToMany.Models
{
    public class Device
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Type")]
        [StringLength(100)]
        public string DeviceType { get; set; }

        public List<CustomerDevice> CustomerDevices { get; set; }
    }
}
