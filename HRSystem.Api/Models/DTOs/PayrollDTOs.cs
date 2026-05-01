// ── Payroll DTOs ──────────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public record PayrollDto(int Id, int EmployeeId, string EmployeeName,
        int PayYear, int PayMonth, decimal BaseSalary, decimal Bonus,
        decimal Allowance, decimal Overtime, decimal Deduction,
        decimal Insurance, decimal Tax, decimal NetSalary,
        byte Status, DateTime? PaidAt, string? Remarks);

    public class CreatePayrollDto
    {
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
        public string? Remarks { get; set; }
    }
}





