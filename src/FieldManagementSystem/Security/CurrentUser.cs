using System.Security.Claims;

namespace FieldManagementSystem.Security;

public static class CurrentUser
{
    public static int GetUserId(ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(value, out var userId))
            throw new InvalidOperationException("Invalid or missing user id claim.");
        return userId;
    }

    public static string GetEmail(ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
}
