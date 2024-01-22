using fInancialFinesseProject.Shared.Domain;

namespace fInancialFinesseProject.Client.Services
{
    interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPosts();
        Task<BlogPost> GetBlogPostByUrl(string url);
    }
}
