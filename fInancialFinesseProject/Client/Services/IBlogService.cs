using fInancialFinesseProject.Shared.Domain;
using System.Dynamic;

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
    }
}
