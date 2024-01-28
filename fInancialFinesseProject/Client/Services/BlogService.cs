﻿using fInancialFinesseProject.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace fInancialFinesseProject.Client.Services
{
    public class BlogService : IBlogService
    {
        private readonly HttpClient _http;

        public BlogService(HttpClient http)
        {
                _http = http;
        }

        public async Task<BlogPost> CreateNewBlogPost(BlogPost request)
        {
            var result = await _http.PostAsJsonAsync("api/Blog", request);  
            return await result.Content.ReadFromJsonAsync<BlogPost>();
        }

        public async Task<BlogPost> GetBlogPostByUrl(string url)
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

        public async Task<List<BlogPost>> GetBlogPosts()
        {
            return await _http.GetFromJsonAsync<List<BlogPost>>($"api/Blog");
        }

        public async Task DeleteBlogPost(int id)
        {
            await _http.DeleteAsync($"api/Blog/{id}");
        }
        public async Task<BlogPost> GetBlogPostById(int id)
        {
            return await _http.GetFromJsonAsync<BlogPost>($"api/Blog/{id}");
        }

        public async Task UpdateBlogPost(BlogPost request)
        {
            await _http.PutAsJsonAsync($"api/Blog/{request.Id}", request);
        }

        public async Task<BlogComment> AddComment(BlogComment comment)
        {
            var response = await _http.PostAsJsonAsync("api/Blog/AddComment", comment);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Adding Comment: {errorMessage}");
                throw new HttpRequestException($"Error Adding Comment: {response.StatusCode} - {errorMessage}");
            }

            return await response.Content.ReadFromJsonAsync<BlogComment>();
        }

        public async Task<List<BlogComment>> GetCommentsByBlogPostId(int blogPostId)
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

        public async Task UpdateComment(BlogComment comment)
        {
            var response = await _http.PutAsJsonAsync($"api/Blog/UpdateComment/{comment.Id}", comment);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteComment(int commentId)
        {
            var response = await _http.DeleteAsync($"api/Blog/DeleteComment/{commentId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
