using fInancialFinesseProject.Shared;
using System.Net.Http.Json;

namespace fInancialFinesseProject.Client.Services2
{
    public class ForumService : IForumService
    {
        private readonly HttpClient _http;
        public ForumService(HttpClient http) //initializes the ForumService with an HttpClient for making HTTP requests.
        {
            _http = http;
        }

        public async Task<ForumPost> CreateNewForumPost(ForumPost request) //Asynchronously sends a POST request to create a new forum post and returns the created post if successful.
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

        public async Task<ForumPost> GetForumPostbyUrl(string url) //Fetches a forum post by its URL. If the post is found, it returns the post; otherwise, it logs an error message and returns a new ForumPost with the error message as the title.
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

        public async Task<List<ForumPost>> GetForumPosts() //Retrieves all forum posts as a list asynchronously.
        {
            return await _http.GetFromJsonAsync<List<ForumPost>>("api/Forum");
        }

        public async Task DeleteForumPost(int id) //Sends a DELETE request to remove a forum post by its ID.
        {
            await _http.DeleteAsync($"api/Forum/{id}");
        }
        public async Task<ForumPost> GetForumPostById(int id) //Fetches a single blog post by its ID.
        {
            return await _http.GetFromJsonAsync<ForumPost>($"api/Forum/{id}");
        }

        public async Task UpdateForumPost(ForumPost request) //Sends a PUT request to update a blog post with new data.
        {
            Console.WriteLine($"Updating ForumPost with Category: {request.Category}");
            await _http.PutAsJsonAsync($"api/Forum/{request.Id}", request);
        }

        public async Task<ForumComment> AddComment(ForumComment comment) //Adds a new comment to a blog post. It sets the related blog post object to null (presumably to avoid circular references in JSON serialization) and sends a POST request.
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

        public async Task<List<ForumComment>> GetCommentsByForumPostId(int forumPostId) //Retrieves all comments for a specific blog post ID.
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

        public async Task UpdateComment(ForumComment comment) //Sends a PUT request to update a specific comment.
        {
            var response = await _http.PutAsJsonAsync($"api/Forum/UpdateComment/{comment.Id}", comment);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Updating Comment: {errorMessage}");
                throw new HttpRequestException($"Error Updating Comment: {response.StatusCode} - {errorMessage}");
            }
        }

        public async Task DeleteComment(int commentId) //Sends a DELETE request to remove a specific comment.
        {
            var response = await _http.DeleteAsync($"api/Forum/DeleteComment/{commentId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<ForumComment> GetCommentById(int commentId) // Fetches a single comment by its ID.
        {
            return await _http.GetFromJsonAsync<ForumComment>($"api/Forum/GetCommentById/{commentId}");
        }

        public async Task<List<ForumCategory>> GetCategories() //Retrieves all blog categories as a list.
        {
            return await _http.GetFromJsonAsync<List<ForumCategory>>("api/Forum/categories");
        }

        public async Task<ForumCategory> CreateCategory(ForumCategory category) // Sends a POST request to create a new category.
        {
            var response = await _http.PostAsJsonAsync("api/Forum/categories", category);
            return await response.Content.ReadFromJsonAsync<ForumCategory>();
        }

        public async Task UpdateCategory(ForumCategory category) //Sends a PUT request to update a specific category.
        {
            await _http.PutAsJsonAsync($"api/Forum/categories/{category.Id}", category);
        }

        public async Task DeleteCategory(int categoryId) //Sends a DELETE request to remove a specific category.
        {
            await _http.DeleteAsync($"api/Forum/categories/{categoryId}");
        }

        public async Task<ForumCategory> GetCategoryById(int categoryId)  //Fetches a single category by its ID.
        {
            return await _http.GetFromJsonAsync<ForumCategory>($"api/Forum/categories/{categoryId}");
        }
    }
}
