using System;
namespace ExampleApiTests;

public class CustomHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage _response;

    public void SetResponse(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_response);
    }
}

