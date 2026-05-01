// =============================================
// Models/Entities/LeaveRequest.cs
// =============================================

namespace HRSystem.Api.Models.Entities;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public byte LeaveType { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Days { get; set; }
    public string? Reason { get; set; }
    public byte Status { get; set; } = 0;
    public int? ApproverId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApproveNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Employee Employee { get; set; } = null!;
    public Employee? Approver { get; set; }
}