using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExampleApiTests;

public class ExampleApiService
{
    private readonly HttpClient _httpClient;
    private readonly string url = "https://jsonplaceholder.typicode.com/posts";

    public ExampleApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Post>>(content);
    }
}

