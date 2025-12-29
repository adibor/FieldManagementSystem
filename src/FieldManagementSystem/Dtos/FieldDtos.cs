using System.ComponentModel.DataAnnotations;

namespace FieldManagementSystem.Dtos
{
    public class FieldCreateRequest
    {
        public string? Name { get; set; }

        [Range(0, double.MaxValue)]
        public double? AreaHectares { get; set; }

        public string? Location { get; set; }
    }

    public class FieldUpdateRequest : FieldCreateRequest { }

    public class FieldPatchRequest
    {
        public string? Name { get; set; }

        [Range(0, double.MaxValue)]
        public double? AreaHectares { get; set; }

        public string? Location { get; set; }
    }

    public class FieldResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double? AreaHectares { get; set; }
        public string? Location { get; set; }
        public int OwnerUserId { get; set; }
    }

    public class FieldTransferRequest
    {
        [Required]
        public int NewOwnerUserId { get; set; }
    }

}
