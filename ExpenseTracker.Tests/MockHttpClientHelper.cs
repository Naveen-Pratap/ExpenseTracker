using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests;

/// <summary>
/// Mock http client helper for mocking in unit tests. 
/// ultimately I decided to go for RichardSzalay.MockHttp
/// </summary>
public static class MockHttpClientHelper
{
    public static HttpClient CreateMockHttpClient<T>(T responseObject, Uri baseUri)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(new HttpResponseMessage
           {
               StatusCode = HttpStatusCode.OK,
               Content = JsonContent.Create(responseObject),
           })
           .Verifiable();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = baseUri
        };

        return httpClient;
    }
}
