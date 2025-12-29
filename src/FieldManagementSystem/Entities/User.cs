using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public ICollection<Field> Fields { get; set; } = new List<Field>();
        public ICollection<UserController> UserControllers { get; set; } = new List<UserController>();
    }
}
