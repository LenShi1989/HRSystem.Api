using AutoMapper;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(HRDbContext db, IConfiguration config) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await db.Users
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

            if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "帳號或密碼錯誤" });

            user.LastLoginAt = DateTime.UtcNow;
            await db.SaveChangesAsync();

            var token = GenerateJwt(user);
            return Ok(new LoginResultDto(token, user.Username, user.Role,
                user.Employee?.FullName));
        }

        private string GenerateJwt(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("role", user.Role.ToString()),
        new Claim("employeeId", user.EmployeeId?.ToString() ?? "")
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

