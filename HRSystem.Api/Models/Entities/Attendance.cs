// =============================================
// Models/Entities/Attendance.cs
// =============================================

namespace HRSystem.Api.Models.Entities;

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly AttendDate { get; set; }
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }
    public decimal? WorkHours { get; set; }
    public byte Status { get; set; } = 1;
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }

    public Employee Employee { get; set; } = null!;
}