using AutoMapper;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.DTOs.Parameters;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LeaveRequestsController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? employeeId,
            [FromQuery] byte? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = db.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.Approver)
                .AsQueryable();

            if (employeeId.HasValue) query = query.Where(l => l.EmployeeId == employeeId.Value);
            if (status.HasValue) query = query.Where(l => l.Status == status.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<LeaveRequestDto>(
                mapper.Map<IEnumerable<LeaveRequestDto>>(items), total, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeaveRequestDto dto)
        {
            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveType = dto.LeaveType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Days = dto.Days,
                Reason = dto.Reason
            };
            db.LeaveRequests.Add(leave);
            await db.SaveChangesAsync();
            return Ok(leave.Id);
        }

        [HttpPatch("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] ApproveLeaveDto dto)
        {
            var leave = await db.LeaveRequests.FindAsync(id);
            if (leave is null) return NotFound();
            if (leave.Status != 0) return BadRequest(new { message = "此申請已處理" });

            leave.Status = dto.Status;
            leave.ApproverId = dto.ApproverId;
            leave.ApprovedAt = DateTime.UtcNow;
            leave.ApproveNote = dto.ApproveNote;
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}
