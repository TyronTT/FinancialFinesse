using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace fInancialFinesseProject.Shared.Domain
{
    public class BlogPost
    {

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, StringLength(100)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } 
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Required, StringLength(20)]
        public string Url { get; set; }
        public String Image { get; set; }
        public string Category { get; set; } = "Uncategorized"; // Default value
    }
}
