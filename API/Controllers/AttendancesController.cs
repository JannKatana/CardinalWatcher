using Application.Attendances;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AttendancesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAttendances()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAttendance(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAttendance(Attendance attendance)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Attendance = attendance }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAttendance(Guid id, Attendance attendance)
        {
            attendance.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Attendance = attendance }));
        }

        [Authorize(Policy = "IsSuperUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("timeIO")]
        public async Task<IActionResult> TimeIO(Attendance attendance)
        {
            return HandleResult(await Mediator.Send(new TimeIO.Command { Attendance = attendance }));
        }

        [HttpPost("filter")]
        public async Task<IActionResult> Filter(FilterDto filter)
        {
            return HandleResult(await Mediator.Send(new Filter.Query { FilterDto = filter }));
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetBlobDownload()
        {
            string filePath = "/WFH TIMEKEEPING REPORT.xlsx";

            return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), Path.GetFileName(filePath));
        }
    }
}
