using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HttpClient.ClientInterfaces;
using HttpClient.Implementations;
using Shared;
using Shared.DTOs;

namespace HttpClients.Implementations;

public class PostHttpClient : IPostService
{
    private readonly System.Net.Http.HttpClient client;

    public PostHttpClient(System.Net.Http.HttpClient client)
    {
        this.client = client;
    }
    
    public async Task CreateAsync(PostCreationDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.PostAsJsonAsync("/posts", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task<ICollection<Post>> GetAsync(int? subFormId, string? title)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        string query = "";
        if (subFormId is not null && subFormId != 0) 
            query += $"?subformid={subFormId}";
        if (!string.IsNullOrEmpty(title))
        {
            query += string.IsNullOrEmpty(query) ? "?" : "&";
            query += $"title={title}";
        }

        HttpResponseMessage response = await client.GetAsync("/posts" + query);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        ICollection<Post> posts = JsonSerializer.Deserialize<ICollection<Post>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return posts;
    }

    public async Task<PostBasicDto> GetByIdAsync(int id)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.GetAsync($"/posts/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        PostBasicDto dto = JsonSerializer.Deserialize<PostBasicDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return dto;
    }

    public async Task UpdateAsync(PostUpdateDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        string json = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("/posts", body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task DeleteAsync(int id)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.DeleteAsync($"/posts/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }
}