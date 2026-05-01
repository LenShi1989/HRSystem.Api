// ── Department DTOs ───────────────────────────────────────────
using System;
using AutoMapper;

namespace HRSystem.Api.Models.DTOs
{
    public record DepartmentDto(int Id, string Name, string Code,
        string? Description, int? ManagerId, string? ManagerName,
        int EmployeeCount, bool IsActive);

    public class CreateDepartmentDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ManagerId { get; set; }
    }
}




