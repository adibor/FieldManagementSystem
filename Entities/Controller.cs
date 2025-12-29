using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Entities
{
    public class Controller
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }
        public string? SerialNumber { get; set; }

        public ICollection<UserController> UserControllers { get; set; } = new List<UserController>();
    }
}
