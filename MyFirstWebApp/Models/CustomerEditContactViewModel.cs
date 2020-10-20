using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyFirstWebApp.Models
{
    public class CustomerEditContactViewModel
    {
        
            [Key]
            public int CustomerId { get; set; }

            [StringLength(24)]
            public string Phone { get; set; }

            [StringLength(24)]
            public string Fax { get; set; }

            [Required]
            [StringLength(60)]
            public string Email { get; set; }
          
    }
}