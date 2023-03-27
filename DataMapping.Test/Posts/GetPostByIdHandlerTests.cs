using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DataMapping.Models.Posts;
using DataMapping.Models.Posts.Exceptions;
using DataMapping.Models.Posts.Handlers;
using DataMapping.Models.Posts.Profiles;
using DataMapping.Models.Posts.Requests;
using DataMapping.Tests.Utils;
using Moq;
using Moq.Protected;
using Xunit;

namespace DataMapping.Tests.Posts.Handlers
{
    public class GetPostByIdHandlerTests
    {
        private readonly IMapper _mapper;

        public GetPostByIdHandlerTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SalidaProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidId_ReturnsMappedPost()
        {
            // Arrange
            var postId = 1;
            var expectedPost = new ServerPost { Id = postId, UserId = 1, Title = "title example", Body = "body example" };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(new List<ServerPost> { expectedPost }))
            };
            var baseAdress = new Uri("https://jsonplaceholder.typicode.com/");
            var httpClientMock = HttpClientUtils.GetHttpClientMock(response, baseAdress);

            var handler = new GetPostByIdHandler(httpClientMock, _mapper);

            // Act
            var result = await handler.Handle(new GetPostByIdRequest(postId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPost.Id, result.Id);
            Assert.Equal(expectedPost.Title, result.Titulo);
        }

        [Fact]
        public async Task Handle_PostNotFound_ThrowsPostNotFoundException()
        {
            // Arrange
            var expectedPostId = 2;
            var posts = new List<ServerPost> { new ServerPost { Id = 1, Title = "title example", Body = "body example" } };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(posts))
            };
            var baseAdress = new Uri("https://jsonplaceholder.typicode.com/");
            var httpClientMock = HttpClientUtils.GetHttpClientMock(response, baseAdress);

            var handler = new GetPostByIdHandler(httpClientMock, _mapper);

            // Act + Assert
            await Assert.ThrowsAsync<PostNotFoundException>(() => handler.Handle(new GetPostByIdRequest(expectedPostId), CancellationToken.None));
        }
    }
}
