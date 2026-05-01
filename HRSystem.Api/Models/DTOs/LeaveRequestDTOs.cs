// ── LeaveRequest DTOs ─────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public record LeaveRequestDto(int Id, int EmployeeId, string EmployeeName,
        byte LeaveType, string LeaveTypeLabel, DateOnly StartDate, DateOnly EndDate,
        decimal Days, string? Reason, byte Status, string StatusLabel,
        int? ApproverId, string? ApproverName, DateTime? ApprovedAt,
        string? ApproveNote, DateTime CreatedAt);

    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }
        public byte LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal Days { get; set; }
        public string? Reason { get; set; }
    }

    public class ApproveLeaveDto
    {
        public byte Status { get; set; }
        public int ApproverId { get; set; }
        public string? ApproveNote { get; set; }
    }
}





