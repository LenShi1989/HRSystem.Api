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
    public class PayrollsController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? employeeId,
            [FromQuery] int? year,
            [FromQuery] int? month,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = db.Payrolls.Include(p => p.Employee).AsQueryable();

            if (employeeId.HasValue) query = query.Where(p => p.EmployeeId == employeeId.Value);
            if (year.HasValue) query = query.Where(p => p.PayYear == year.Value);
            if (month.HasValue) query = query.Where(p => p.PayMonth == month.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.PayYear).ThenByDescending(p => p.PayMonth)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<PayrollDto>(
                mapper.Map<IEnumerable<PayrollDto>>(items), total, page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePayrollDto dto)
        {
            if (await db.Payrolls.AnyAsync(p =>
                p.EmployeeId == dto.EmployeeId &&
                p.PayYear == dto.PayYear && p.PayMonth == dto.PayMonth))
                return BadRequest(new { message = "此月份薪資已存在" });

            var payroll = new Payroll
            {
                EmployeeId = dto.EmployeeId,
                PayYear = dto.PayYear,
                PayMonth = dto.PayMonth,
                BaseSalary = dto.BaseSalary,
                Bonus = dto.Bonus,
                Allowance = dto.Allowance,
                Overtime = dto.Overtime,
                Deduction = dto.Deduction,
                Insurance = dto.Insurance,
                Tax = dto.Tax,
                NetSalary = dto.BaseSalary + dto.Bonus + dto.Allowance + dto.Overtime
                             - dto.Deduction - dto.Insurance - dto.Tax,
                Remarks = dto.Remarks
            };
            db.Payrolls.Add(payroll);
            await db.SaveChangesAsync();
            return Ok(payroll.Id);
        }

        [HttpPatch("{id}/pay")]
        public async Task<IActionResult> MarkPaid(int id)
        {
            var payroll = await db.Payrolls.FindAsync(id);
            if (payroll is null) return NotFound();
            payroll.Status = 1;
            payroll.PaidAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

}
