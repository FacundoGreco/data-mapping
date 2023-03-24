using DataMapping.Models.Posts;
using DataMapping.Models.Posts.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
                var posts = await _mediator.Send(new GetPostsRequest());

                if(posts == null)
                {
                    return NotFound("No hay posts guardados.");
                }
                
                var post = posts.FirstOrDefault(p => p.Id.Equals(id));

                if(post == null)
                {
                    return NotFound("No existe un post con ese ID.");
                }


                
                return Ok(post);

            }catch (Exception ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ha ocurrido un error.");
            }
        }
    }
}
