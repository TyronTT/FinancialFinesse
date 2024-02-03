using fInancialFinesseProject.Shared;
using System.Net.Http.Json;

namespace fInancialFinesseProject.Client.Services2
{
    public class ForumService : IForumService
    {
        private readonly HttpClient _http;
        public ForumService(HttpClient http) 
        {
            _http = http;
        }

        public async Task<ForumPost> CreateNewForumPost(ForumPost request)
        {
            var response = await _http.PostAsJsonAsync("api/Forum", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ForumPost>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Error in CreateNewForumPost: {error}");
            }
        }

        public async Task<ForumPost> GetForumPostbyUrl(string url)
        {
            //var post = await _http.GetFromJsonAsync<ForumPost>($"api/Forum/{url}");
            //return post;

            var result = await _http.GetAsync($"api/Forum/{url}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                Console.WriteLine(message);
                return new ForumPost { Title = message };
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<ForumPost>();
            }
        }

        public async Task<List<ForumPost>> GetForumPosts()
        {
            return await _http.GetFromJsonAsync<List<ForumPost>>("api/Forum");
        }

        public async Task DeleteForumPost(int id)
        {
            await _http.DeleteAsync($"api/Forum/{id}");
        }
        public async Task<ForumPost> GetForumPostById(int id)
        {
            return await _http.GetFromJsonAsync<ForumPost>($"api/Forum/{id}");
        }

        public async Task UpdateForumPost(ForumPost request)
        {
            Console.WriteLine($"Updating ForumPost with Category: {request.Category}");
            await _http.PutAsJsonAsync($"api/Forum/{request.Id}", request);
        }

        public async Task<ForumComment> AddComment(ForumComment comment)
        {
            comment.ForumPost = null;
            var response = await _http.PostAsJsonAsync("api/Forum/AddComment", comment);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Adding Comment: {errorMessage}");
                throw new HttpRequestException($"Error Adding Comment: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<ForumComment>();
        }

        public async Task<List<ForumComment>> GetCommentsByForumPostId(int forumPostId)
        {
            var response = await _http.GetAsync($"api/Forum/GetComments/{forumPostId}");
            if (!response.IsSuccessStatusCode)
            {
                // Handle the response accordingly
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<ForumComment>(); // or throw an exception
            }

            return await response.Content.ReadFromJsonAsync<List<ForumComment>>();
        }

        public async Task UpdateComment(ForumComment comment)
        {
            var response = await _http.PutAsJsonAsync($"api/Forum/UpdateComment/{comment.Id}", comment);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Updating Comment: {errorMessage}");
                throw new HttpRequestException($"Error Updating Comment: {response.StatusCode} - {errorMessage}");
            }
        }

        public async Task DeleteComment(int commentId)
        {
            var response = await _http.DeleteAsync($"api/Forum/DeleteComment/{commentId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ForumComment> GetCommentById(int commentId)
        {
            return await _http.GetFromJsonAsync<ForumComment>($"api/Forum/GetCommentById/{commentId}");
        }

        public async Task<List<ForumCategory>> GetCategories()
        {
            return await _http.GetFromJsonAsync<List<ForumCategory>>("api/Forum/categories");
        }

        public async Task<ForumCategory> CreateCategory(ForumCategory category)
        {
            var response = await _http.PostAsJsonAsync("api/Forum/categories", category);
            return await response.Content.ReadFromJsonAsync<ForumCategory>();
        }

        public async Task UpdateCategory(ForumCategory category)
        {
            await _http.PutAsJsonAsync($"api/Forum/categories/{category.Id}", category);
        }

        public async Task DeleteCategory(int categoryId)
        {
            await _http.DeleteAsync($"api/Forum/categories/{categoryId}");
        }

        public async Task<ForumCategory> GetCategoryById(int categoryId)
        {
            return await _http.GetFromJsonAsync<ForumCategory>($"api/Forum/categories/{categoryId}");
        }
    }
}
