using AutoMapper;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DepartmentsController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var depts = await db.Departments
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .Where(d => d.IsActive)
                .OrderBy(d => d.Code)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<DepartmentDto>>(depts));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await db.Departments
                .Include(d => d.Manager)
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dept is null) return NotFound();
            return Ok(mapper.Map<DepartmentDto>(dept));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            if (await db.Departments.AnyAsync(d => d.Code == dto.Code))
                return BadRequest(new { message = "部門代碼已存在" });

            var dept = mapper.Map<Department>(dto);
            db.Departments.Add(dept);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = dept.Id }, dept.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDepartmentDto dto)
        {
            var dept = await db.Departments.FindAsync(id);
            if (dept is null) return NotFound();
            mapper.Map(dto, dept);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }

}




