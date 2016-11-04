using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToMany.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(100)]
        public string CustomerDisplayName { get; set; }

        public List<int> SelectedDevices { get; set; }
        public SelectList Devices { get; set; }
    }
}
