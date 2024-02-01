﻿using fInancialFinesseProject.Server.Data;
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
        private readonly ILogger<BlogController> _logger;

        public BlogController(DataContext context, ILogger<BlogController> logger) 
        { 
            _context = context;
            _logger = logger;
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
            post.CategoryId = request.CategoryId;
            post.Category = request.Category;
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

        [HttpPut("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] BlogComment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingComment = await _context.Comments.FindAsync(id);
            if (existingComment == null)
            {
                return NotFound();
            }

            existingComment.Text = comment.Text;
            existingComment.BlogPostId = comment.BlogPostId;

            _context.Entry(existingComment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("GetCommentById/{id}")]
        public async Task<ActionResult<BlogComment>> GetCommentById(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                _logger.LogInformation($"Comment not found with ID: {id}");
                return NotFound();
            }
            _logger.LogInformation($"Fetched comment: {comment.Id}, Author: {comment.Author}, Text: {comment.Text}");
            return Ok(comment);
        }

        [HttpGet("categories")]
        public ActionResult<List<BlogCategory>> GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        // Create a new category
        [HttpPost("categories")]
        public async Task<ActionResult<BlogCategory>> CreateCategory([FromBody] BlogCategory category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // Get a category by ID
        [HttpGet("categories/{id}")]
        public ActionResult<BlogCategory> GetCategoryById(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return category;
        }

        // Update a category
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] BlogCategory category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete a category
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
