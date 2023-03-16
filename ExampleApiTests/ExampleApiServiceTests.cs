using System.Net;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;

namespace ExampleApiTests;

public class ExampleApiServiceTests
{
    private readonly CustomHttpMessageHandler _customHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly ExampleApiService _exapmpleApiService;

    public ExampleApiServiceTests()
    {
        _customHttpMessageHandler = new CustomHttpMessageHandler();
        _httpClient = new HttpClient(_customHttpMessageHandler);
        _exapmpleApiService = new ExampleApiService(_httpClient);
    }


    [Fact]
    public async Task GetAllPostsAsync_ShouldReturnAllPosts_WhenApiCallSucceeds()
    {
        // Arrange
        var posts = new List<Post>
            {
                new Post { UserId = 1, Id = 1, Title = "Example Title 1", Body = "Example Body 1" },
                new Post { UserId = 2, Id = 2, Title = "Example Title 2", Body = "Example Body 2" },
            };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(posts), Encoding.UTF8, "application/json"),
        };
        _customHttpMessageHandler.SetResponse(httpResponse);

        // Act
        var result = await _exapmpleApiService.GetAllPostsAsync();

        // Assert
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async Task GetAllPostsAsync_ShouldThrowHttpRequestException_WhenApiCallFails()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError,
        };
        _customHttpMessageHandler.SetResponse(httpResponse);

        // Act
        Func<Task> act = async () => await _exapmpleApiService.GetAllPostsAsync();

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetAllPostsAsync_ShouldThrowJsonReaderException_WhenInvalidJsonIsReceived()
    {
        // Arrange
        var invalidJson = "{ this is an invalid json }";
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(invalidJson, Encoding.UTF8, "application/json"),
        };
        _customHttpMessageHandler.SetResponse(httpResponse);

        // Act
        Func<Task> act = async () => await _exapmpleApiService.GetAllPostsAsync();

        // Assert
        await act.Should().ThrowAsync<JsonReaderException>();
    }
}
