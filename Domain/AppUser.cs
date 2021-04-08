using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string Suffix { get; set; } = string.Empty;
        public string Department { get; set; }
        public bool IsPresent { get; set; } = false;
        public bool IsSuperUser { get; set; } = false;

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
