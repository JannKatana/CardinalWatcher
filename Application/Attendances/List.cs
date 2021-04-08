using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attendances
{
    public class List
    {
        public class Query : IRequest<Result<List<AttendanceDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<AttendanceDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<AttendanceDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                    x.UserName == _userAccessor.GetUsername());

                List<AttendanceDto> attendances;
                if (user.IsSuperUser)
                {
                    attendances = await _context.Attendances
                        .Where(x => x.AppUser.IsPresent == true && x.TimeIn.Date == DateTime.Now.Date)
                        .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);

                }
                else
                {
                    DateTime startDateFilter = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    DateTime endDateFilter = startDateFilter.AddMonths(1);
                    attendances = await _context.Attendances
                        .Where(x => x.AppUser == user && x.TimeIn >= startDateFilter && x.TimeIn < endDateFilter)
                        .ProjectTo<AttendanceDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
                }

                return Result<List<AttendanceDto>>.Success(attendances);
            }
        }
    }
}
