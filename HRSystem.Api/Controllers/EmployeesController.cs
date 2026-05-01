using AutoMapper;
using HRSystem.Api.Models;
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
    public class EmployeesController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EmployeeQueryParams q)
        {
            var query = db.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q.Keyword))
                query = query.Where(e =>
                    e.FullName.Contains(q.Keyword) ||
                    e.EmployeeNo.Contains(q.Keyword) ||
                    e.Email.Contains(q.Keyword));

            if (q.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId == q.DepartmentId.Value);

            if (q.PositionId.HasValue)
                query = query.Where(e => e.PositionId == q.PositionId.Value);

            if (q.Status.HasValue)
                query = query.Where(e => e.Status == q.Status.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.EmployeeNo)
                .Skip((q.Page - 1) * q.PageSize)
                .Take(q.PageSize)
                .ToListAsync();

            return Ok(new PagedResult<EmployeeListDto>(
                mapper.Map<IEnumerable<EmployeeListDto>>(items), total, q.Page, q.PageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await db.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emp is null) return NotFound();
            return Ok(mapper.Map<EmployeeDetailDto>(emp));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (await db.Employees.AnyAsync(e => e.EmployeeNo == dto.EmployeeNo))
                return BadRequest(new { message = "員工編號已存在" });

            if (await db.Employees.AnyAsync(e => e.Email == dto.Email))
                return BadRequest(new { message = "Email 已存在" });

            var emp = mapper.Map<Employee>(dto);
            db.Employees.Add(emp);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = emp.Id }, emp.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            var emp = await db.Employees.FindAsync(id);
            if (emp is null) return NotFound();

            mapper.Map(dto, emp);
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "4")]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await db.Employees.FindAsync(id);
            if (emp is null) return NotFound();

            emp.Status = 3; // 標記為離職而非實際刪除
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = new
            {
                Total = await db.Employees.CountAsync(),
                Active = await db.Employees.CountAsync(e => e.Status == 1),
                ByDepartment = await db.Departments
                    .Select(d => new
                    {
                        d.Name,
                        Count = d.Employees.Count(e => e.Status == 1)
                    }).ToListAsync(),
                NewThisMonth = await db.Employees.CountAsync(e =>
                    e.HireDate.Month == DateTime.Now.Month &&
                    e.HireDate.Year == DateTime.Now.Year)
            };
            return Ok(stats);
        }
    }

}
