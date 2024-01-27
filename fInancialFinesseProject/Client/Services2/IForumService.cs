using fInancialFinesseProject.Shared;


using fInancialFinesseProject.Shared;

namespace fInancialFinesseProject.Client.Services2
{
    public interface IForumService
    {
        Task<List<ForumPost>> GetForumPosts();
        Task<ForumPost> GetForumPostbyUrl(string url);
        Task<ForumPost> CreateNewForumPost(ForumPost request);
        Task DeleteForumPost(int id);
        Task<ForumPost> GetForumPostById(int id);
        Task UpdateForumPost(ForumPost forumPost);
    }
}
