using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Dtos
{
    public class AuthLoginRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class AuthLoginResponse
    {
        public string Token { get; set; } = string.Empty;
    }


}
