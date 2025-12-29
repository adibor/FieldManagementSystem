using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Dtos
{
    public class ControllerCreateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Type { get; set; }
        public string? SerialNumber { get; set; }
    }

    public class ControllerUpdateRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Type { get; set; }
        public string? SerialNumber { get; set; }
    }

    public class ControllerPatchRequest
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? SerialNumber { get; set; }
    }

    public class ControllerResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? SerialNumber { get; set; }
    }

    public class AssignControllerRequest
    {
        [Required]
        public int UserId { get; set; }

        public string? Role { get; set; }
    }
}
