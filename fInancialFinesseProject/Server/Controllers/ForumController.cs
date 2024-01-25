using fInancialFinesseProject.Server.Data;
using fInancialFinesseProject.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace fInancialFinesseProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly ForumDataContext _context;

        public ForumController(ForumDataContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ForumPost>> GimmeAllTheForumPosts()
        {
            return Ok(_context.ForumPosts);
        }

        [HttpGet("{url}")]
        public ActionResult<ForumPost> GimmeThatSingleForumPost(string url)
        {
            var post = _context.ForumPosts.FirstOrDefault(p => p.Url.ToLower().Equals(url.ToLower()));
            if(post == null)
            {
                return NotFound("This Forum does not exist.");
            }

            return Ok(post);
        }

        [HttpPost]
        public async Task<ActionResult<ForumPost>> CreateNewForumPost(ForumPost request)
        {
            _context.Add(request);
            await _context.SaveChangesAsync();

            return request;
        }
    }
}
