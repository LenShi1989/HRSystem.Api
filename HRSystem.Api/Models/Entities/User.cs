// =============================================
// Models/Entities/User.cs
// =============================================

namespace HRSystem.Api.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int? EmployeeId { get; set; }
    public byte Role { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Employee? Employee { get; set; }
}