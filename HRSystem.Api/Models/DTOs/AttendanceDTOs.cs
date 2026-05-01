// ── Attendance DTOs ───────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public record AttendanceDto(int Id, int EmployeeId, string EmployeeName,
        DateOnly AttendDate, TimeOnly? CheckIn, TimeOnly? CheckOut,
        decimal? WorkHours, byte Status, string StatusLabel, string? Remarks);

    public class UpsertAttendanceDto
    {
        public int EmployeeId { get; set; }
        public DateOnly AttendDate { get; set; }
        public TimeOnly? CheckIn { get; set; }
        public TimeOnly? CheckOut { get; set; }
        public byte Status { get; set; } = 1;
        public string? Remarks { get; set; }
    }
}





