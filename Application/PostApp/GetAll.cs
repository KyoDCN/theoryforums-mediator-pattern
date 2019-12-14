using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.PostApp
{
    public class GetAll
    {
        public class Query : IRequest<List<GetDTO>>
        {
            public int ThreadId { get; set; }
        }

        public class ValidateQuery : AbstractValidator<Query>
        {
            public ValidateQuery()
            {
                RuleFor(x => x.ThreadId).GreaterThanOrEqualTo(0);
            }
        }

        public class Handler : IRequestHandler<Query, List<GetDTO>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<GetDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Posts
                    .Where(x => x.ThreadFK == request.ThreadId)
                    .Select(x => new GetDTO
                    {
                        Id = x.Id,
                        Content = x.Content,
                        CreatedOn = x.CreatedOn,
                        Edited = x.Edited,
                        LastEditDate = x.LastEditDate,
                        Author = new GetDTO.UserDTO
                        {
                            Id = x.Author.Id,
                            DisplayName = x.Author.DisplayName,
                            AvatarUrl = x.Author.AvatarUrl
                        }
                    })
                    .ToListAsync();

                //return await (from post in _context.Posts
                //              where post.ThreadFK == request.ThreadId
                //              select new GetDTO
                //              {
                //                  Id = post.Id,
                //                  Content = post.Content,
                //                  CreatedOn = post.CreatedOn,
                //                  Edited = post.Edited,
                //                  LastEditDate = post.LastEditDate,
                //                  Author = (from user in _context.Users
                //                            where user.Id == post.AuthorFK
                //                            select new GetDTO.UserDTO
                //                            {
                //                                Id = user.Id,
                //                                DisplayName = user.DisplayName,
                //                                AvatarUrl = user.AvatarUrl
                //                            }).SingleOrDefault()
                //              }).ToListAsync();
            }
        }
    }
}
