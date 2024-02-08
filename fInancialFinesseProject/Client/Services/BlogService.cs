using fInancialFinesseProject.Shared.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace fInancialFinesseProject.Client.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _http;

        public BlogService(HttpClient http) //initializes the BlogService with an HttpClient for making HTTP requests.
        {
            _http = http;
        }

        public async Task<BlogPost> CreateNewBlogPost(BlogPost request) //Asynchronously sends a POST request to create a new blog post and returns the created post if successful.
        {
            var response = await _http.PostAsJsonAsync("api/Blog", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<BlogPost>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Error in CreateNewBlogPost: {error}");
            }
        }

        public async Task<BlogPost> GetBlogPostByUrl(string url) //Fetches a blog post by its URL. If the post is found, it returns the post; otherwise, it logs an error message and returns a new BlogPost with the error message as the title.
        {
            //var post = await _http.GetFromJsonAsync<BlogPost>($"api/Blog/{url}");
            //return post;

            var result = await _http.GetAsync($"api/Blog/{url}");
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var message = await result.Content.ReadAsStringAsync();
                Console.WriteLine(message);
                return new BlogPost { Title = message };
            }
            else
            {
                return await result.Content.ReadFromJsonAsync<BlogPost>();
            }
        }

        public async Task<List<BlogPost>> GetBlogPosts() //Retrieves all blog posts as a list asynchronously.
        {
            return await _http.GetFromJsonAsync<List<BlogPost>>($"api/Blog");
        }

        public async Task DeleteBlogPost(int id) //Sends a DELETE request to remove a blog post by its ID.
        {
            await _http.DeleteAsync($"api/Blog/{id}");
        }
        public async Task<BlogPost> GetBlogPostById(int id) //Fetches a single blog post by its ID.
        {
            return await _http.GetFromJsonAsync<BlogPost>($"api/Blog/{id}");
        }

        public async Task UpdateBlogPost(BlogPost request) //Sends a PUT request to update a blog post with new data.
        {
            Console.WriteLine($"Updating BlogPost with Category: {request.Category}");
            await _http.PutAsJsonAsync($"api/Blog/{request.Id}", request);
        }

        public async Task<BlogComment> AddComment(BlogComment comment) //Adds a new comment to a blog post. It sets the related blog post object to null (presumably to avoid circular references in JSON serialization) and sends a POST request.
        {
            comment.BlogPost = null;
            var response = await _http.PostAsJsonAsync("api/Blog/AddComment", comment);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Adding Comment: {errorMessage}");
                throw new HttpRequestException($"Error Adding Comment: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<BlogComment>();
        }

        public async Task<List<BlogComment>> GetCommentsByBlogPostId(int blogPostId) //Retrieves all comments for a specific blog post ID.
        {
            var response = await _http.GetAsync($"api/Blog/GetComments/{blogPostId}");
            if (!response.IsSuccessStatusCode)
            {
                // Handle the response accordingly
                Console.WriteLine($"Error: {response.StatusCode}");
                return new List<BlogComment>(); // or throw an exception
            }

            return await response.Content.ReadFromJsonAsync<List<BlogComment>>();
        }

        public async Task UpdateComment(BlogComment comment) //Sends a PUT request to update a specific comment.
        {
            var response = await _http.PutAsJsonAsync($"api/Blog/UpdateComment/{comment.Id}", comment);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Updating Comment: {errorMessage}");
                throw new HttpRequestException($"Error Updating Comment: {response.StatusCode} - {errorMessage}");
            }
        }

        public async Task DeleteComment(int commentId) //Sends a DELETE request to remove a specific comment.
        {
            var response = await _http.DeleteAsync($"api/Blog/DeleteComment/{commentId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<BlogComment> GetCommentById(int commentId) // Fetches a single comment by its ID.
        {
            return await _http.GetFromJsonAsync<BlogComment>($"api/Blog/GetCommentById/{commentId}");
        }

        public async Task<List<BlogCategory>> GetCategories() //Retrieves all blog categories as a list.
        {
            return await _http.GetFromJsonAsync<List<BlogCategory>>("api/Blog/categories");
        }

        public async Task<BlogCategory> CreateCategory(BlogCategory category) // Sends a POST request to create a new category.
        {
            var response = await _http.PostAsJsonAsync("api/Blog/categories", category);
            return await response.Content.ReadFromJsonAsync<BlogCategory>();
        }

        public async Task UpdateCategory(BlogCategory category) //Sends a PUT request to update a specific category.
        {
            await _http.PutAsJsonAsync($"api/Blog/categories/{category.Id}", category);
        }

        public async Task DeleteCategory(int categoryId) //Sends a DELETE request to remove a specific category.
        {
            await _http.DeleteAsync($"api/Blog/categories/{categoryId}");
        }

        public async Task<BlogCategory> GetCategoryById(int categoryId) //Fetches a single category by its ID.
        {
            return await _http.GetFromJsonAsync<BlogCategory>($"api/Blog/categories/{categoryId}");
        }
    }
}
