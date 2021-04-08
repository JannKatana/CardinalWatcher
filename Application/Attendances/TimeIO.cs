using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attendances
{
    public class TimeIO
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Attendance Attendance { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Attendance).SetValidator(new AttendanceValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                _context = context;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetUsername());

                if (!user.IsPresent)
                {
                    var attendance = request.Attendance;
                    attendance.AppUser = user;
                    attendance.TimeIn = System.DateTime.Now;
                    _context.Attendances.Add(attendance);
                }
                else
                {
                    var attendance = await _context.Attendances.FindAsync(request.Attendance.Id);

                    if (attendance == null) return null;

                    attendance.TimeOut = System.DateTime.Now;
                    attendance.Accomplishments = request.Attendance.Accomplishments;
                    attendance.AppUser = user;
                }

                user.IsPresent = !user.IsPresent;
                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create attendance");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
