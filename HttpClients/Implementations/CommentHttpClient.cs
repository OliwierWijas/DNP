using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using HttpClient.ClientInterfaces;
using Shared;
using Shared.DTOs;

namespace HttpClient.Implementations;

public class CommentHttpClient : ICommentService
{
    private readonly System.Net.Http.HttpClient client;

    public CommentHttpClient(System.Net.Http.HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateAsync(CommentCreationDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.PostAsJsonAsync("/comments", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task<ICollection<Comment>> GetAsync(int postId)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.GetAsync($"/comments?postid={postId}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        ICollection<Comment> comments = JsonSerializer.Deserialize<ICollection<Comment>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return comments;
    }
}