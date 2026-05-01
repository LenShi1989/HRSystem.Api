// =============================================
// Models/Entities/Employee.cs
// =============================================

namespace HRSystem.Api.Models.Entities;

public class Employee
{
    public int Id { get; set; }
    public string EmployeeNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public byte Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string? IdCardNo { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Photo { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
    public int? ManagerId { get; set; }
    public DateOnly HireDate { get; set; }
    public DateOnly? ResignDate { get; set; }
    public byte EmploymentType { get; set; } = 1;
    public byte Status { get; set; } = 1;
    public decimal BaseSalary { get; set; }
    public string? BankAccount { get; set; }
    public string? EmergencyName { get; set; }
    public string? EmergencyPhone { get; set; }
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Department Department { get; set; } = null!;
    public Position Position { get; set; } = null!;
    public Employee? Manager { get; set; }
    public ICollection<Employee> Subordinates { get; set; } = new List<Employee>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public string FullName => $"{LastName}{FirstName}";
}