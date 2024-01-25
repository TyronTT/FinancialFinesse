using fInancialFinesseProject.Server.Data;
using fInancialFinesseProject.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;


namespace fInancialFinesseProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly DataContext _context;
       
        public BlogController(DataContext context) 
        { 
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<BlogPost>> GimmeAllTheBlogPosts()
        {
            return Ok(_context.BlogPosts.OrderByDescending(post => post.DateCreated));
        }

        [HttpGet("{url}")]
        public ActionResult<BlogPost> GimmeThatSingleBlogPost(string url)
        {
            var post = _context.BlogPosts.FirstOrDefault(p => p.Url.ToLower().Equals(url.ToLower()));
            if(post == null)
            {
                return NotFound("This Post Does Not Exist.");
            }

            return Ok(post);
        }
       
        [HttpPost]
        public async Task<ActionResult<BlogPost>> CreateNewBlogPost(BlogPost request)
        {
            _context.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }
    }
}
