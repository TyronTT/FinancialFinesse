using fInancialFinesseProject.Shared.Domain;
using System.Dynamic;
using System.Xml.Linq;

namespace fInancialFinesseProject.Client.Services
{
    interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPosts();
        Task<BlogPost> GetBlogPostByUrl(string url);
        Task<BlogPost> CreateNewBlogPost(BlogPost request);
        Task DeleteBlogPost(int id);
        Task<BlogPost> GetBlogPostById(int id);
        Task UpdateBlogPost(BlogPost blogPost);

        // Add a new comment
        Task<BlogComment> AddComment(BlogComment comment);

        // Retrieve comments for a specific blog post
        Task<List<BlogComment>> GetCommentsByBlogPostId(int blogPostId);

        // Update a comment
        Task UpdateComment(BlogComment comment);

        // Delete a comment
        Task DeleteComment(int commentId);

        Task<BlogComment> GetCommentById(int commentId);

        Task<List<BlogCategory>> GetCategories();
        Task<BlogCategory> CreateCategory(BlogCategory category);
        Task UpdateCategory(BlogCategory category);
        Task DeleteCategory(int categoryId);
        Task<BlogCategory> GetCategoryById(int categoryId);
    }
}
