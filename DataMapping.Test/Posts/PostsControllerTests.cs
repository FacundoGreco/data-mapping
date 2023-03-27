using DataMapping.Controllers;
using DataMapping.Models.Posts;
using DataMapping.Models.Posts.Exceptions;
using DataMapping.Models.Posts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DataMapping.Tests.Posts
{
    public class PostsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ILogger<PostsController> _loggerMock;
        private readonly PostsController _controller;

        public PostsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<PostsController>>().Object;
            _controller = new PostsController(_loggerMock, _mediatorMock.Object);
        }

        [Fact]
        public async Task GetPostById_ReturnsOkResult_WhenPostExists()
        {
            // Arrange
            var postId = 1;
            var post = new Salida { Id = postId, Titulo = "Test Post" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPostByIdRequest>(), default))
                         .ReturnsAsync(post);

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var jsonString = Assert.IsType<string>(okResult.Value);
            var returnedPost = Assert.IsType<Salida>(JsonSerializer.Deserialize<Salida>(jsonString));
            
            Assert.Equal(post.Id, returnedPost.Id);
            Assert.Equal(post.Titulo, returnedPost.Titulo);
        }

        [Fact]
        public async Task GetPostById_ReturnsNotFoundResult_WhenPostDoesNotExist()
        {
            // Arrange
            var postId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPostByIdRequest>(), default))
                         .ThrowsAsync(new PostNotFoundException());

            // Act
            var result = await _controller.GetPostById(postId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }

}
