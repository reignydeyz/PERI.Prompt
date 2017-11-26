using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.Web.Models
{
    public class Search
    {
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Query { get; set; }
    }
}
