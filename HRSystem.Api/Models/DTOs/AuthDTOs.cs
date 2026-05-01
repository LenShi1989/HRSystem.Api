// ── Auth DTOs ─────────────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public record LoginResultDto(string Token, string Username, byte Role, string? EmployeeName);
}




