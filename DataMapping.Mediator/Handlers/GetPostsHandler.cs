using DataMapping.Models.Posts.Requests;
using MediatR;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataMapping.Models.Posts.Handlers
{
    public class GetPostsHandler : IRequestHandler<GetPostsRequest, List<ServerPost>>
    {
        private readonly HttpClient _httpClient;

        public GetPostsHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ServerPost>> Handle(GetPostsRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts", cancellationToken);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<List<ServerPost>>(json);
        }
    }
}
