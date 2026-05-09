using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.UserRoleDtos
{
    public class UserRoleFilter
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public int? RoleId { get; set; }
    }
}
