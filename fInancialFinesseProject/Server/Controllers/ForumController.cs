using fInancialFinesseProject.Server.Data;
using fInancialFinesseProject.Shared;
using fInancialFinesseProject.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return Ok(_context.ForumPosts.OrderByDescending(post => post.DateCreated));
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteForumPost(int id)
        {
            var post = _context.ForumPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            _context.ForumPosts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent(); // Indicates successful deletion
        }

        [HttpGet("{id:int}")]
        public ActionResult<ForumPost> GetForumPostById(int id)
        {
            var forumPost = _context.ForumPosts.FirstOrDefault(p => p.Id == id);
            if (forumPost == null)
            {
                return NotFound();
            }
            return Ok(forumPost);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ForumPost>> UpdateForumPost(int id, ForumPost request)
        {
            var post = _context.ForumPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Update properties
            post.Author = request.Author;
            post.Title = request.Title;
            post.Url = request.Url;
            post.Description = request.Description;
            post.Image = request.Image;
            post.Content = request.Content;
            post.DateCreated = request.DateCreated;
            post.IsPublished = request.IsPublished;
            // Update other properties as needed

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(post);
        }
    }
}
