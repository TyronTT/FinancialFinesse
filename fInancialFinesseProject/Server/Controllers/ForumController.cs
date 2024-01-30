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
        private readonly ILogger<ForumController> _logger;

        public ForumController(ForumDataContext context, ILogger<ForumController> logger) 
        {
            _context = context;
            _logger = logger;
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

        [HttpPost("AddComment")]
        public async Task<ActionResult<ForumComment>> AddComment(ForumComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            comment.ForumPost = _context.ForumPosts.Find(comment.ForumPostId);
            _context.forumComments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpGet("GetComments/{forumPostId}")]
        public ActionResult<List<ForumComment>> GetCommentsByForumPostId(int forumPostId)
        {
            var comments = _context.forumComments.Where(c => c.ForumPostId == forumPostId).OrderByDescending(c => c.DatePosted).ToList();

            if (!comments.Any())
            {
                return NotFound("No comments found for this post.");
            }

            return Ok(comments);
        }

        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] ForumComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingComment = await _context.forumComments.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Text = comment.Text;
            existingComment.ForumPostId = comment.ForumPostId;

            _context.Entry(existingComment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _context.forumComments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.forumComments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GetCommentById/{id}")]
        public async Task<ActionResult<ForumComment>> GetCommentById(int id)
        {
            var comment = await _context.forumComments.FindAsync(id);
            if (comment == null)
            {
                _logger.LogInformation($"Comment not found with ID: {id}");
                return NotFound();
            }
            _logger.LogInformation($"Fetched comment: {comment.Id}, Author: {comment.Author}, Text: {comment.Text}");
            return Ok(comment);
        }
    }
}
