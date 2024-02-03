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

        // Add a new comment
        Task<ForumComment> AddComment(ForumComment comment);

        // Retrieve comments for a specific blog post
        Task<List<ForumComment>> GetCommentsByForumPostId(int ForumPostId);

        // Update a comment
        Task UpdateComment(ForumComment comment);

        // Delete a comment
        Task DeleteComment(int commentId);

        Task<ForumComment> GetCommentById(int commentId);
        Task<List<ForumCategory>> GetCategories();
        Task<ForumCategory> CreateCategory(ForumCategory category);
        Task UpdateCategory(ForumCategory category);
        Task DeleteCategory(int categoryId);
        Task<ForumCategory> GetCategoryById(int categoryId);
    }
}
