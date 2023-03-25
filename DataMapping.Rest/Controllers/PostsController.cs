using DataMapping.Models.Posts;
using DataMapping.Models.Posts.Exceptions;
using DataMapping.Models.Posts.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataMapping.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IMediator _mediator;

        public PostsController(ILogger<PostsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Salida>> GetPostById([FromQuery][Required] int id)
        {
            try
            {
                var post = await _mediator.Send(new GetPostByIdRequest(id));
                return Ok(JsonSerializer.Serialize(post));

            }catch (EmptyPostsException ex)
            {
                _logger.LogInformation(ex, "No hay posts guardados.");
                return NotFound("No hay posts guardados.");
            }
            catch (PostNotFoundException ex)
            {
                _logger.LogInformation(ex, "No existe un post con ese ID.");
                return NotFound("No existe un post con ese ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ha ocurrido un error.");
            }
        }
    }
}
