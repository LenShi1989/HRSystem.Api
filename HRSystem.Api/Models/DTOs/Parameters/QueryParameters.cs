// ── Query Parameters ──────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs.Parameters
{
    public class EmployeeQueryParams
    {
        public string? Keyword { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public byte? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public record PagedResult<T>(IEnumerable<T> Items, int Total, int Page, int PageSize);
}





