using AutoMapper;
using HRSystem.Api.Models.DTOs;
using HRSystem.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PositionsController(HRDbContext db, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var positions = await db.Positions
                .Where(p => p.IsActive)
                .OrderBy(p => p.Level)
                .ToListAsync();
            return Ok(mapper.Map<IEnumerable<PositionDto>>(positions));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePositionDto dto)
        {
            if (await db.Positions.AnyAsync(p => p.Code == dto.Code))
                return BadRequest(new { message = "職位代碼已存在" });

            var pos = mapper.Map<Position>(dto);
            db.Positions.Add(pos);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { }, pos.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePositionDto dto)
        {
            var pos = await db.Positions.FindAsync(id);
            if (pos is null) return NotFound();
            mapper.Map(dto, pos);
            await db.SaveChangesAsync();
            return NoContent();
        }
    }
}


