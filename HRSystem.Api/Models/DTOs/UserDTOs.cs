namespace HRSystem.Api.Models.DTOs
{
    public record UserDto(
        int Id,
        string Username,
        int? EmployeeId,
        string? EmployeeName,
        byte Role,
        bool IsActive,
        DateTime? LastLoginAt,
        DateTime CreatedAt
    );

    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public byte Role { get; set; } = 1;
        public bool IsActive { get; set; } = true;
    }

    public class UpdateUserDto
    {
        public int? EmployeeId { get; set; }
        public byte Role { get; set; } = 1;
        public bool IsActive { get; set; } = true;
    }

    public class ResetUserPasswordDto
    {
        public string Password { get; set; } = string.Empty;
    }
}
