namespace FieldManagementSystem.Entities
{
    public class UserController
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ControllerId { get; set; }
        public Controller Controller { get; set; } = null!;

        public string? Role { get; set; }

    }
}
