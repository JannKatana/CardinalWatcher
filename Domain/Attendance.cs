using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string Accomplishments { get; set; }
        public AppUser AppUser { get; set; }
    }
}