using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UserDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string  Email { get; set; }
        public string DisplayName { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }
        public bool IsPresent { get; set; }
        public bool IsSuperUser { get; set; } = false;
    }
}
