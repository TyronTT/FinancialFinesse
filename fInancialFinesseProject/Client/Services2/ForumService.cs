using fInancialFinesseProject.Shared;
using fInancialFinesseProject.Shared.Domain;
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
            var result = await _http.PostAsJsonAsync("api/Forum", request);
            return await result.Content.ReadFromJsonAsync<ForumPost>();
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
            await _http.PutAsJsonAsync($"api/Forum/{request.Id}", request);
        }
    }
}
