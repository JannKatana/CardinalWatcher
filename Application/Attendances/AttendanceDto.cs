using Application.Profiles;
using System;

namespace Application.Attendances
{
    public class AttendanceDto
    {
        public Guid Id { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string Accomplishments { get; set; }
        public Profile Profile { get; set; }
    }
}
