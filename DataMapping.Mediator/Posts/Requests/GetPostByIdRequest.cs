using MediatR;
using System.Collections.Generic;

namespace DataMapping.Models.Posts.Requests
{
    public class GetPostByIdRequest : IRequest<Salida>
    {
        public int Id { get; }

        public GetPostByIdRequest(int id)
        {
            this.Id = id;
        }
    }
}
