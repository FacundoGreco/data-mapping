using Moq.Protected;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace DataMapping.Tests.Utils
{
    public class HttpClientUtils
    {
        public static HttpClient GetHttpClientMock(HttpResponseMessage response, Uri baseAdress)
        {
            var messageHandlerMock = new Mock<HttpMessageHandler>();
            messageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            return new HttpClient(messageHandlerMock.Object)
            {
                BaseAddress = baseAdress
            };
        }
    }
}
