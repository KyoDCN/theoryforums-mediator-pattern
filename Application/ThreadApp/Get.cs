using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ThreadApp
{
    public class Get
    {
        public class Query : IRequest<GetDTO>
        {
            public int Id { get; set; }
        }

        public class ValidateQuery : AbstractValidator<Query>
        {
            public ValidateQuery()
            {
                RuleFor(x => x.Id).GreaterThanOrEqualTo(0).WithMessage("Invalid thread.");
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
                return await (from thread in _context.Threads
                              where thread.Id == request.Id
                              select new GetDTO
                              {
                                  Id = thread.Id,
                                  Title = thread.Title,
                                  Content = thread.Content,
                                  CreatedOn = thread.CreatedOn,
                                  Edited = thread.Edited,
                                  LastEditDate = thread.LastEditDate,
                                  Forum = (from forum in _context.Forums
                                           where forum.Id == thread.ForumFK
                                           select new GetDTO.ForumDTO
                                           {
                                               Id = forum.Id,
                                               Title = forum.Title,
                                               Slug = forum.Slug,
                                               Icon = forum.Icon,
                                           }).SingleOrDefault(),
                                  Author = (from user in _context.Users
                                            where user.Id == thread.AuthorFK
                                            select new GetDTO.UserDTO
                                            {
                                                Id = user.Id,
                                                DisplayName = user.DisplayName,
                                                AvatarUrl = user.AvatarUrl
                                            }).SingleOrDefault()
                              }).FirstOrDefaultAsync();
            }
        }
    }
}
