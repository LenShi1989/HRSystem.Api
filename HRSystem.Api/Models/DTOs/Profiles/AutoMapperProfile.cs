// ── AutoMapper Profile ────────────────────────────────────────

using AutoMapper;
using HRSystem.Api.Models;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.Entities;


namespace HRSystem.Api.Models.DTOs.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeListDto>()
                .ConstructUsing(e => new EmployeeListDto(
                    e.Id, e.EmployeeNo, e.FullName, e.Email, e.Phone,
                    e.Department.Name, e.Position.Title,
                    GetStatusLabel(e.Status), e.Status, e.HireDate, e.BaseSalary));

            CreateMap<Employee, EmployeeDetailDto>()
                .ConstructUsing(e => new EmployeeDetailDto(
                    e.Id, e.EmployeeNo, e.FirstName, e.LastName, e.FullName,
                    e.Gender, e.BirthDate, e.IdCardNo, e.Email, e.Phone,
                    e.Address, e.Photo,
                    e.DepartmentId, e.Department.Name,
                    e.PositionId, e.Position.Title,
                    e.ManagerId, e.Manager != null ? e.Manager.FullName : null,
                    e.HireDate, e.ResignDate,
                    e.EmploymentType, e.Status,
                    e.BaseSalary, e.BankAccount,
                    e.EmergencyName, e.EmergencyPhone, e.Remarks,
                    e.CreatedAt, e.UpdatedAt));

            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();

            CreateMap<Department, DepartmentDto>()
                .ConstructUsing(d => new DepartmentDto(
                    d.Id, d.Name, d.Code, d.Description,
                    d.ManagerId, d.Manager != null ? d.Manager.FullName : null,
                    d.Employees.Count(e => e.Status == 1), d.IsActive));

            CreateMap<CreateDepartmentDto, Department>();

            CreateMap<Position, PositionDto>()
                .ConstructUsing(p => new PositionDto(
                    p.Id, p.Title, p.Code, p.Level,
                    p.MinSalary, p.MaxSalary, p.Description, p.IsActive));

            CreateMap<CreatePositionDto, Position>();

            CreateMap<Attendance, AttendanceDto>()
                .ConstructUsing(a => new AttendanceDto(
                    a.Id, a.EmployeeId, a.Employee.FullName,
                    a.AttendDate, a.CheckIn, a.CheckOut, a.WorkHours,
                    a.Status, GetAttendanceStatusLabel(a.Status), a.Remarks));

            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ConstructUsing(l => new LeaveRequestDto(
                    l.Id, l.EmployeeId, l.Employee.FullName,
                    l.LeaveType, GetLeaveTypeLabel(l.LeaveType),
                    l.StartDate, l.EndDate, l.Days, l.Reason,
                    l.Status, GetLeaveStatusLabel(l.Status),
                    l.ApproverId, l.Approver != null ? l.Approver.FullName : null, l.ApprovedAt,
                    l.ApproveNote, l.CreatedAt));

            CreateMap<Payroll, PayrollDto>()
                .ConstructUsing(p => new PayrollDto(
                    p.Id, p.EmployeeId, p.Employee.FullName,
                    p.PayYear, p.PayMonth, p.BaseSalary, p.Bonus,
                    p.Allowance, p.Overtime, p.Deduction,
                    p.Insurance, p.Tax, p.NetSalary,
                    p.Status, p.PaidAt, p.Remarks));
        }

        private static string GetStatusLabel(byte status) => status switch
        {
            1 => "在職",
            2 => "留職停薪",
            3 => "離職",
            _ => "未知"
        };

        private static string GetAttendanceStatusLabel(byte status) => status switch
        {
            1 => "正常",
            2 => "遲到",
            3 => "早退",
            4 => "缺勤",
            5 => "公假",
            6 => "事假",
            7 => "病假",
            _ => "未知"
        };

        private static string GetLeaveTypeLabel(byte type) => type switch
        {
            1 => "年假",
            2 => "事假",
            3 => "病假",
            4 => "婚假",
            5 => "喪假",
            6 => "產假",
            7 => "陪產假",
            _ => "其他"
        };

        private static string GetLeaveStatusLabel(byte status) => status switch
        {
            0 => "待審核",
            1 => "已核准",
            2 => "已拒絕",
            3 => "已撤銷",
            _ => "未知"
        };
    }

}





