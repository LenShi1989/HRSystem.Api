// ── Position DTOs ─────────────────────────────────────────────

namespace HRSystem.Api.Models.DTOs
{
    public record PositionDto(int Id, string Title, string Code, int Level,
        decimal? MinSalary, decimal? MaxSalary, string? Description, bool IsActive);

    public class CreatePositionDto
    {
        public string Title { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Description { get; set; }
    }
}





