using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumCategoryApp
{
    public class GetAll
    {
        public class Query : IRequest<List<GetAllDTO>>
        {
        }

        public class Handler : IRequestHandler<Query, List<GetAllDTO>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<GetAllDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await (from category in _context.ForumCategories
                              select new GetAllDTO
                              {
                                  Id = category.Id,
                                  Title = category.Title,
                                  Slug = category.Slug,
                                  Description = category.Description,
                                  Forums =
                                  (from forums in _context.Forums
                                   where forums.ForumCategoryFK == category.Id
                                   select new GetAllDTO.Forum
                                   {
                                       Id = forums.Id,
                                       Title = forums.Title,
                                       Slug = forums.Slug,
                                       Description = forums.Description,
                                       Icon = forums.Icon,
                                       LatestReply =
                                       (from thread in _context.Threads
                                        where thread.ForumFK == forums.Id
                                        orderby thread.LastUpdated descending
                                        select new GetAllDTO.Forum.Reply
                                        {
                                            ThreadId = thread.Id,
                                            ThreadTitle = thread.Title,
                                            ThreadSlug = thread.Slug,
                                            PostReplyDate = thread.LastUpdated,
                                            Author =
                                            (from user in _context.Users
                                             where user.Id == thread.AuthorFK
                                             select new GetAllDTO.Forum.Reply.User
                                             {
                                                 Id = user.Id,
                                                 DisplayName = user.DisplayName,
                                                 AvatarUrl = user.AvatarUrl
                                             }).SingleOrDefault()
                                        }).SingleOrDefault()
                                   }).ToList()
                              }).ToListAsync();
            }
        }
    }
}
