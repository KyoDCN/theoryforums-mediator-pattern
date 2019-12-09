using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ForumApp
{
    public class Get
    {
        public class Query : IRequest<GetDTO>
        {
            public int Id { get; set; }
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
                var query = from forum in _context.Forums
                            where forum.Id == request.Id
                            select new GetDTO
                            {
                                Id = forum.Id,
                                Title = forum.Title,
                                Slug = forum.Slug,
                                Description = forum.Description,
                                Icon = forum.Icon
                            };

                return await query.SingleOrDefaultAsync();
            }
        }

    }
}
