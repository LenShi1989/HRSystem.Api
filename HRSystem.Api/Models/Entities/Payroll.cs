// =============================================
// Models/Entities/Payroll.cs
// =============================================

namespace HRSystem.Api.Models.Entities;

public class Payroll
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int PayYear { get; set; }
    public int PayMonth { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Allowance { get; set; }
    public decimal Overtime { get; set; }
    public decimal Deduction { get; set; }
    public decimal Insurance { get; set; }
    public decimal Tax { get; set; }
    public decimal NetSalary { get; set; }
    public byte Status { get; set; } = 0;
    public DateTime? PaidAt { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Employee Employee { get; set; } = null!;
}
