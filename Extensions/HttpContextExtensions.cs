namespace FieldManagementSystem.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserEmail(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["X-User-Email"].FirstOrDefault()
                ?? throw new Exception("X-User-Email header is missing");
        }
    }
}
