using AutoMapper;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.DTOs.Parameters;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendancesController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? employeeId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 30)
        {
            var query = db.Attendances
                .Include(a => a.Employee)
                .AsQueryable();

            if (employeeId.HasValue) query = query.Where(a => a.EmployeeId == employeeId.Value);
            if (startDate.HasValue) query = query.Where(a => a.AttendDate >= startDate.Value);
            if (endDate.HasValue) query = query.Where(a => a.AttendDate <= endDate.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.AttendDate)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<AttendanceDto>(
                mapper.Map<IEnumerable<AttendanceDto>>(items), total, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] UpsertAttendanceDto dto)
        {
            var existing = await db.Attendances
                .FirstOrDefaultAsync(a => a.EmployeeId == dto.EmployeeId && a.AttendDate == dto.AttendDate);

            if (existing is null)
            {
                var att = new Attendance
                {
                    EmployeeId = dto.EmployeeId,
                    AttendDate = dto.AttendDate,
                    CheckIn = dto.CheckIn,
                    CheckOut = dto.CheckOut,
                    Status = dto.Status,
                    Remarks = dto.Remarks
                };
                if (att.CheckIn.HasValue && att.CheckOut.HasValue)
                    att.WorkHours = (decimal)(att.CheckOut.Value - att.CheckIn.Value).TotalHours;
                db.Attendances.Add(att);
            }
            else
            {
                existing.CheckIn = dto.CheckIn;
                existing.CheckOut = dto.CheckOut;
                existing.Status = dto.Status;
                existing.Remarks = dto.Remarks;
                if (existing.CheckIn.HasValue && existing.CheckOut.HasValue)
                    existing.WorkHours = (decimal)(existing.CheckOut.Value - existing.CheckIn.Value).TotalHours;
            }

            await db.SaveChangesAsync();
            return Ok();
        }
    }

}
