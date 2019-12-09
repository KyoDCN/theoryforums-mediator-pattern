using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumApp
{
    public class GetAll
    {
        public class Query : IRequest<List<GetDTO>>
        {
            public int ForumCategoryId { get; set; }
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
                var query = from forums in _context.Forums
                            where forums.ForumCategoryFK == request.ForumCategoryId
                            select new GetDTO
                            {
                                Id = forums.Id,
                                Title = forums.Title,
                                Slug = forums.Slug,
                                Description = forums.Description,
                                Icon = forums.Icon
                            };

                return await query.ToListAsync();
            }
        }

    }
}
