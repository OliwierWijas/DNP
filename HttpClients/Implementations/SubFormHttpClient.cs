using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using HttpClient.ClientInterfaces;
using Shared;
using Shared.DTOs;

namespace HttpClient.Implementations;

public class SubFormHttpClient : ISubFormService
{
    private readonly System.Net.Http.HttpClient client;

    public SubFormHttpClient(System.Net.Http.HttpClient client)
    {
        this.client = client;
    }

    public async Task CreateAsync(SubFormBasicDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.PostAsJsonAsync("/subForms", dto);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task<ICollection<SubForm>> GetAsync(string username, string? name)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        string query = $"?username={username}&name={name}";

        HttpResponseMessage response = await client.GetAsync("/subforms" + query);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        ICollection<SubForm> subForms = JsonSerializer.Deserialize<ICollection<SubForm>>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        return subForms;
    }

    public async Task<SubFormBasicDto> GetByIdAsync(int id)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.GetAsync($"/subforms/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);

        SubFormBasicDto dto = JsonSerializer.Deserialize<SubFormBasicDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        return dto;
    }

    public async Task UpdateAsync(SubFormUpdateDto dto)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        string json = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("/subforms", body);
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }

    public async Task DeleteAsync(int id)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);
        HttpResponseMessage response = await client.DeleteAsync($"/subforms/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
    }
}