using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.PostApp
{
    public class Get
    {
        public class Query : IRequest<GetDTO>
        {
            public int PostId { get; set; }
        }

        public class ValidateQuery : AbstractValidator<Query>
        {
            public ValidateQuery()
            {
                RuleFor(x => x.PostId).GreaterThanOrEqualTo(0);
            }
        }

        public class Handler : IRequestHandler<Query, GetDTO>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<GetDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                return await (from post in _context.Posts
                              where post.Id == request.PostId
                              select new GetDTO
                              {
                                  Id = post.Id,
                                  Content = post.Content,
                                  CreatedOn = post.CreatedOn,
                                  Edited = post.Edited,
                                  LastEditDate = post.LastEditDate,
                                  Author = (from user in _context.Users
                                            where user.Id == post.AuthorFK
                                            select new GetDTO.UserDTO
                                            {
                                                Id = user.Id,
                                                DisplayName = user.DisplayName,
                                                AvatarUrl = user.AvatarUrl
                                            }).SingleOrDefault()
                              }).SingleOrDefaultAsync();
            }
        }
    }
}
