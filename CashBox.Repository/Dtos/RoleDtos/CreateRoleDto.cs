using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Dtos.RoleDtos
{
    public class CreateRoleDto
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(300)]
        public string Description { get; set; } = null!;
    }
}
