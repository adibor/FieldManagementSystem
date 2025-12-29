using FieldManagementSystem.Data;
using FieldManagementSystem.Dtos;
using FieldManagementSystem.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FieldManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FieldsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FieldsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<FieldResponse>>> Get()
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            var query = _db.Fields.AsNoTracking();

            if (!isAdmin)
                query = query.Where(f => f.OwnerUserId == userId);

            var list = await query
                .OrderByDescending(f => f.Id)
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    AreaHectares = f.AreaHectares,
                    Location = f.Location,
                    OwnerUserId = f.OwnerUserId
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FieldResponse>> GetById(int id)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            var query = _db.Fields.AsNoTracking().Where(f => f.Id == id);
            if (!isAdmin)
                query = query.Where(f => f.OwnerUserId == userId);

            var field = await query
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    AreaHectares = f.AreaHectares,
                    Location = f.Location,
                    OwnerUserId = f.OwnerUserId
                })
                .FirstOrDefaultAsync();

            if (field == null) return NotFound();
            return Ok(field);
        }

        [HttpPost]
        public async Task<ActionResult<FieldResponse>> Create([FromBody] FieldCreateRequest request)
        {
            var userId = CurrentUser.GetUserId(User);

            var field = new Entities.Field
            {
                Name = request.Name,
                AreaHectares = request.AreaHectares,
                Location = request.Location,
                OwnerUserId = userId
            };

            _db.Fields.Add(field);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = field.Id }, new FieldResponse
            {
                Id = field.Id,
                Name = field.Name,
                AreaHectares = field.AreaHectares,
                Location = field.Location,
                OwnerUserId = field.OwnerUserId
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] FieldUpdateRequest request)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            var field = await _db.Fields.FirstOrDefaultAsync(f => f.Id == id && (isAdmin || f.OwnerUserId == userId));
            if (field == null) return NotFound();

            field.Name = request.Name;
            field.AreaHectares = request.AreaHectares;
            field.Location = request.Location;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] FieldPatchRequest request)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            var field = await _db.Fields.FirstOrDefaultAsync(f => f.Id == id && (isAdmin || f.OwnerUserId == userId));
            if (field == null) return NotFound();

            if (request.Name != null) field.Name = request.Name;
            if (request.AreaHectares.HasValue) field.AreaHectares = request.AreaHectares;
            if (request.Location != null) field.Location = request.Location;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            var field = await _db.Fields.FirstOrDefaultAsync(f => f.Id == id && (isAdmin || f.OwnerUserId == userId));
            if (field == null) return NotFound();

            _db.Fields.Remove(field);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Admin-only transfer ownership
        [HttpPost("{id:int}/transfer")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> TransferOwnership(int id, [FromBody] FieldTransferRequest request)
        {
            var field = await _db.Fields.FirstOrDefaultAsync(f => f.Id == id);
            if (field == null) return NotFound();

            var newOwnerExists = await _db.Users.AnyAsync(u => u.Id == request.NewOwnerUserId);
            if (!newOwnerExists)
                return BadRequest($"User {request.NewOwnerUserId} does not exist.");

            if (field.OwnerUserId == request.NewOwnerUserId)
                return NoContent();

            field.OwnerUserId = request.NewOwnerUserId;
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }
}
