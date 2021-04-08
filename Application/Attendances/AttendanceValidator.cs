using Domain;
using FluentValidation;

namespace Application.Attendances
{
    public class AttendanceValidator : AbstractValidator<Attendance>
    {
        public AttendanceValidator()
        {
            RuleFor(x => x.TimeIn).NotEmpty();
        }

    }
}
