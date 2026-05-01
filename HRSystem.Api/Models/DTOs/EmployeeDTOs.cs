// ── Employee DTOs ─────────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public record EmployeeListDto(
        int Id, string EmployeeNo, string FullName, string Email,
        string? Phone, string DepartmentName, string PositionTitle,
        string StatusLabel, byte Status, DateOnly HireDate, decimal BaseSalary
    );

    public record EmployeeDetailDto(
        int Id, string EmployeeNo, string FirstName, string LastName,
        string FullName, byte Gender, DateOnly? BirthDate, string? IdCardNo,
        string Email, string? Phone, string? Address, string? Photo,
        int DepartmentId, string DepartmentName,
        int PositionId, string PositionTitle,
        int? ManagerId, string? ManagerName,
        DateOnly HireDate, DateOnly? ResignDate,
        byte EmploymentType, byte Status,
        decimal BaseSalary, string? BankAccount,
        string? EmergencyName, string? EmergencyPhone, string? Remarks,
        DateTime CreatedAt, DateTime UpdatedAt
    );

    public class CreateEmployeeDto
    {
        public string EmployeeNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? IdCardNo { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int? ManagerId { get; set; }
        public DateOnly HireDate { get; set; }
        public byte EmploymentType { get; set; } = 1;
        public decimal BaseSalary { get; set; }
        public string? BankAccount { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdateEmployeeDto : CreateEmployeeDto
    {
        public byte Status { get; set; } = 1;
        public DateOnly? ResignDate { get; set; }
    }
}





