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
    public class ControllersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ControllersController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<ControllerResponse>>> Get()
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            if (isAdmin)
            {
                var all = await _db.Controllers.AsNoTracking()
                    .OrderByDescending(c => c.Id)
                    .Select(c => new ControllerResponse
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type,
                        SerialNumber = c.SerialNumber
                    })
                    .ToListAsync();

                return Ok(all);
            }

            var mine = await _db.UserControllers.AsNoTracking()
                .Where(uc => uc.UserId == userId)
                .Select(uc => new ControllerResponse
                {
                    Id = uc.Controller.Id,
                    Name = uc.Controller.Name,
                    Type = uc.Controller.Type,
                    SerialNumber = uc.Controller.SerialNumber
                })
                .Distinct()
                .ToListAsync();

            return Ok(mine);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ControllerResponse>> GetById(int id)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            if (!isAdmin)
            {
                var hasAccess = await _db.UserControllers.AsNoTracking()
                    .AnyAsync(uc => uc.UserId == userId && uc.ControllerId == id);

                if (!hasAccess) return NotFound();
            }

            var controller = await _db.Controllers.AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new ControllerResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    SerialNumber = c.SerialNumber
                })
                .FirstOrDefaultAsync();

            if (controller == null) return NotFound();
            return Ok(controller);
        }

        [HttpPost]
        public async Task<ActionResult<ControllerResponse>> Create([FromBody] ControllerCreateRequest request)
        {
            var userId = CurrentUser.GetUserId(User);

            var controller = new Entities.Controller
            {
                Name = request.Name.Trim(),
                Type = request.Type,
                SerialNumber = request.SerialNumber
            };

            _db.Controllers.Add(controller);
            await _db.SaveChangesAsync();

            _db.UserControllers.Add(new Entities.UserController
            {
                UserId = userId,
                ControllerId = controller.Id,
                Role = "Owner"
            });

            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = controller.Id }, new ControllerResponse
            {
                Id = controller.Id,
                Name = controller.Name,
                Type = controller.Type,
                SerialNumber = controller.SerialNumber
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ControllerUpdateRequest request)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            if (!isAdmin)
            {
                var hasAccess = await _db.UserControllers.AnyAsync(uc => uc.UserId == userId && uc.ControllerId == id);
                if (!hasAccess) return NotFound();
            }

            var controller = await _db.Controllers.FirstOrDefaultAsync(c => c.Id == id);
            if (controller == null) return NotFound();

            controller.Name = request.Name.Trim();
            controller.Type = request.Type;
            controller.SerialNumber = request.SerialNumber;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] ControllerPatchRequest request)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            if (!isAdmin)
            {
                var hasAccess = await _db.UserControllers.AnyAsync(uc => uc.UserId == userId && uc.ControllerId == id);
                if (!hasAccess) return NotFound();
            }

            var controller = await _db.Controllers.FirstOrDefaultAsync(c => c.Id == id);
            if (controller == null) return NotFound();

            if (request.Name != null) controller.Name = request.Name.Trim();
            if (request.Type != null) controller.Type = request.Type;
            if (request.SerialNumber != null) controller.SerialNumber = request.SerialNumber;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = CurrentUser.GetUserId(User);
            var isAdmin = CurrentUser.IsAdmin(User);

            if (!isAdmin)
            {
                var isOwner = await _db.UserControllers.AnyAsync(uc =>
                    uc.UserId == userId && uc.ControllerId == id && uc.Role == "Owner");

                if (!isOwner) return Forbid();
            }

            var controller = await _db.Controllers.FirstOrDefaultAsync(c => c.Id == id);
            if (controller == null) return NotFound();

            var links = _db.UserControllers.Where(uc => uc.ControllerId == id);
            _db.UserControllers.RemoveRange(links);

            _db.Controllers.Remove(controller);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{controllerId:int}/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Assign(int controllerId, [FromBody] AssignControllerRequest request)
        {
            var exists = await _db.Controllers.AnyAsync(c => c.Id == controllerId);
            if (!exists) return NotFound("Controller not found.");

            var userExists = await _db.Users.AnyAsync(u => u.Id == request.UserId);
            if (!userExists) return NotFound("User not found.");

            var already = await _db.UserControllers.AnyAsync(uc => uc.UserId == request.UserId && uc.ControllerId == controllerId);
            if (already) return Conflict("Already assigned.");

            _db.UserControllers.Add(new Entities.UserController
            {
                UserId = request.UserId,
                ControllerId = controllerId,
                Role = request.Role
            });

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{controllerId:int}/unassign")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unassign(int controllerId, [FromQuery] int userId)
        {
            var link = await _db.UserControllers.FirstOrDefaultAsync(x => x.UserId == userId && x.ControllerId == controllerId);
            if (link == null) return NotFound();

            _db.UserControllers.Remove(link);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{controllerId:int}/unassign-me")]
        public async Task<IActionResult> UnassignMe(int controllerId)
        {
            var userId = CurrentUser.GetUserId(User);

            var link = await _db.UserControllers
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ControllerId == controllerId);

            if (link == null) return NotFound();

            _db.UserControllers.Remove(link);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
