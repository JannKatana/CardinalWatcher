using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attendances
{
    public class Details
    {
        public class Query : IRequest<Result<AttendanceDto>> {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<AttendanceDto>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<AttendanceDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var attendance = await _context.Attendances
                    .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<AttendanceDto>.Success(attendance);
            }
        }
    }
}
