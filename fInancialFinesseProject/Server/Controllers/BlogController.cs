using fInancialFinesseProject.Server.Data;
using fInancialFinesseProject.Shared.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;

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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogPost(int id)
        {
            var post = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent(); // Indicates successful deletion
        }

        [HttpGet("{id:int}")]
        public ActionResult<BlogPost> GetBlogPostById(int id)
        {
            var blogPost = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return Ok(blogPost);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BlogPost>> UpdateBlogPost(int id, BlogPost request)
        {
            var post = _context.BlogPosts.FirstOrDefault(p => p.Id == id);
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

        [HttpPost("AddComment")]
        public async Task<ActionResult<BlogComment>> AddComment(BlogComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment.BlogPost = _context.BlogPosts.Find(comment.BlogPostId);
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpGet("GetComments/{blogPostId}")]
        public ActionResult<List<BlogComment>> GetCommentsByBlogPostId(int blogPostId)
        {
            var comments = _context.Comments.Where(c => c.BlogPostId == blogPostId).OrderByDescending(c => c.DatePosted).ToList();

            if (!comments.Any())
            {
                return NotFound("No comments found for this post.");
            }

            return Ok(comments);
        }
    }
}
