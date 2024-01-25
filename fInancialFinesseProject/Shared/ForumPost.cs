using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fInancialFinesseProject.Shared
{
    public class ForumPost
    {
        public int Id { get; set; }

        [Required]
        public String Title { get; set; }

        [Required]
        public String Content { get; set; }

        [Required, StringLength(100)]
        public String Description { get; set; }

        [Required]
        public string Author { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Required, StringLength(20)]
        public string Url { get; set; }
        public string Image { get;set; }
    }
}
