using fInancialFinesseProject.Shared.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fInancialFinesseProject.Shared
{
    public class ForumComment
    {
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Text { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.Now;

        [Required]
        public int ForumPostId { get; set; }

        public ForumPost? ForumPost { get; set; }
    }
}
