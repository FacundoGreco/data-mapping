using AutoMapper;
using DataMapping.Models.Posts.Exceptions;
using DataMapping.Models.Posts.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DataMapping.Models.Posts.Handlers
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdRequest, Salida>
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public GetPostByIdHandler(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<Salida> Handle(GetPostByIdRequest request, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts", cancellationToken);
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var posts = JsonSerializer.Deserialize<List<ServerPost>>(json);

            ValidatePosts(posts, request.Id);

            var post = posts.FirstOrDefault(p => p.Id.Equals(request.Id));
            return _mapper.Map<Salida>(post);
        }

        private void ValidatePosts(List<ServerPost> posts, int id)
        {
            if (posts == null || posts.Count == 0)
            {
                throw new EmptyPostsException();
            }

            if (!posts.Any(sp => sp.Id.Equals(id)))
            {
                throw new PostNotFoundException();
            }
        }
    }
}
