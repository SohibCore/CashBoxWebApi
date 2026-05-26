using RepositoryLayer.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity
{
    [Table("SYS_PRODUCT")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("NAME")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("CODE")]
        [StringLength(10)]
        public string Code { get; set; } = null!;

        [Required]
        [Column("ORGANIZATION_ID")]
        public int OrganizationId { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        [Required]
        [Column("DELIVERED_AT")]
        public DateTime DeliveredAt { get; set; }

        [Column("CREATED_USER_ID")]
        public int? CreatedUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }
    }
}
