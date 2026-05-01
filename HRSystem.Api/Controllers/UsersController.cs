using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "4")]
    public class UsersController(HRDbContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await db.Users
                .Include(u => u.Employee)
                .OrderBy(u => u.Username)
                .Select(u => new UserDto(
                    u.Id,
                    u.Username,
                    u.EmployeeId,
                    u.Employee != null ? u.Employee.FullName : null,
                    u.Role,
                    u.IsActive,
                    u.LastLoginAt,
                    u.CreatedAt))
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await db.Users
                .Include(u => u.Employee)
                .Where(u => u.Id == id)
                .Select(u => new UserDto(
                    u.Id,
                    u.Username,
                    u.EmployeeId,
                    u.Employee != null ? u.Employee.FullName : null,
                    u.Role,
                    u.IsActive,
                    u.LastLoginAt,
                    u.CreatedAt))
                .FirstOrDefaultAsync();

            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var username = dto.Username.Trim();

            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { message = "請輸入帳號" });

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return BadRequest(new { message = "密碼至少 6 碼" });

            if (!IsValidRole(dto.Role))
                return BadRequest(new { message = "角色不正確" });

            if (await db.Users.AnyAsync(u => u.Username == username))
                return BadRequest(new { message = "帳號已存在" });

            if (dto.EmployeeId.HasValue && !await db.Employees.AnyAsync(e => e.Id == dto.EmployeeId.Value))
                return BadRequest(new { message = "綁定員工不存在" });

            if (dto.EmployeeId.HasValue && await db.Users.AnyAsync(u => u.EmployeeId == dto.EmployeeId.Value))
                return BadRequest(new { message = "此員工已綁定其他帳號" });

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                EmployeeId = dto.EmployeeId,
                Role = dto.Role,
                IsActive = dto.IsActive,
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) return NotFound();

            if (!IsValidRole(dto.Role))
                return BadRequest(new { message = "角色不正確" });

            if (dto.EmployeeId.HasValue && !await db.Employees.AnyAsync(e => e.Id == dto.EmployeeId.Value))
                return BadRequest(new { message = "綁定員工不存在" });

            if (dto.EmployeeId.HasValue && await db.Users.AnyAsync(u =>
                u.EmployeeId == dto.EmployeeId.Value && u.Id != id))
                return BadRequest(new { message = "此員工已綁定其他帳號" });

            user.EmployeeId = dto.EmployeeId;
            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetUserPasswordDto dto)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) return NotFound();

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return BadRequest(new { message = "密碼至少 6 碼" });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await db.SaveChangesAsync();

            return NoContent();
        }

        private static bool IsValidRole(byte role) => role is >= 1 and <= 4;
    }
}
