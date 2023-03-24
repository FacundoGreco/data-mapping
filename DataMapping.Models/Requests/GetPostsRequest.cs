using MediatR;
using System.Collections.Generic;

namespace DataMapping.Models.Posts.Requests
{
    public class GetPostsRequest : IRequest<List<ServerPost>>
    {
    }
}
