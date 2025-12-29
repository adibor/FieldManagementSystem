using System.Security.Claims;

namespace FieldManagementSystem.Extensions
{
    public static class CurrentUser
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var value = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value
                        ?? user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? user.FindFirstValue(ClaimTypes.Name);

            if (!int.TryParse(value, out var id))
                throw new InvalidOperationException("User id claim missing or invalid.");

            return id;
        }

        public static bool IsAdmin(ClaimsPrincipal user)
            => user.IsInRole("Admin");
    }
}
