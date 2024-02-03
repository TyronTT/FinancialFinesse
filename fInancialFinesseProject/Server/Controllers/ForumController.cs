using fInancialFinesseProject.Server.Data;
using fInancialFinesseProject.Shared;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.fCategories
                                        .FirstOrDefaultAsync(c => c.Id == request.CategoryId);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId");
            }

            request.Category = category.Name;

            _context.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetForumPostById), new { id = request.Id }, request);
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
            Console.WriteLine($"Received Category to Update: {request.Category}");
            var post = _context.ForumPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Update properties
            post.Author = request.Author;
            post.Title = request.Title;
            post.Url = request.Url;
            post.CategoryId = request.CategoryId;
            // Fetch the new category name based on CategoryId
            var category = await _context.fCategories.FirstOrDefaultAsync(c => c.Id == request.CategoryId);
            if (category != null)
            {
                post.Category = category.Name; // Update the category name
            }
            else
            {
                // Handle the case where the new CategoryId does not correspond to an existing category
                Console.WriteLine("Invalid CategoryId provided.");
                return BadRequest("Invalid CategoryId provided.");
            }
            post.Description = request.Description;
            post.Image = request.Image;
            post.Content = request.Content;
            post.DateCreated = request.DateCreated;
            post.IsPublished = request.IsPublished;
            // Update other properties as needed

            Console.WriteLine(post.Category);

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

        [HttpGet("categories")]
        public ActionResult<List<ForumCategory>> GetCategories()
        {
            var categories = _context.fCategories.ToList();
            return Ok(categories);
        }

        // Create a new category
        [HttpPost("categories")]
        public async Task<ActionResult<ForumCategory>> CreateCategory([FromBody] ForumCategory category)
        {
            _context.fCategories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // Get a category by ID
        [HttpGet("categories/{id}")]
        public async Task<ActionResult<ForumCategory>> GetCategoryById(int id)
        {
            var category = await _context.fCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // Update a category
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ForumCategory categoryUpdate)
        {
            //if (id != category.Id)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //await _context.SaveChangesAsync();
            //return NoContent();

            var category = await _context.fCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound("Category not found.");
            }

            if (id != categoryUpdate.Id)
            {
                return BadRequest("Mismatched Category ID.");
            }

            // Check if the category name is actually being updated to avoid unnecessary operations
            if (category.Name != categoryUpdate.Name)
            {
                // Update the category name
                category.Name = categoryUpdate.Name;

                // Now, update all forum posts that are linked to this category
                var postsToUpdate = await _context.ForumPosts.Where(p => p.CategoryId == id).ToListAsync();
                foreach (var post in postsToUpdate)
                {
                    post.Category = categoryUpdate.Name;
                }
            }

            // No need to set the entity state to Modified for tracked entities when their properties have been changed
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete a category
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.fCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Find all blog posts associated with this category
            var postsToUpdate = _context.ForumPosts.Where(p => p.CategoryId == id);

            // Set the Category of affected posts to null or a default value
            foreach (var post in postsToUpdate)
            {
                post.Category = "Uncategorized"; // Or set to a default category name if preferred
                post.CategoryId = 0;
            }

            _context.fCategories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
