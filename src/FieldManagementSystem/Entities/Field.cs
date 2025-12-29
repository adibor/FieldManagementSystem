using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Entities
{
    public class Field
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public double? AreaHectares { get; set; }

        // Logical / descriptive location (not GPS coordinates)
        public string? Location { get; set; }

        public int OwnerUserId { get; set; }
        public User OwnerUser { get; set; } = null!;
    }
}
