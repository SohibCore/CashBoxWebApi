using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity
{
    [Table("SYS_SUPPLIER")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public int Id { get; set; }

        [Required]
        [Column("INN")]
        [StringLength(10)]
        public string Inn { get; set; } = null!;

        [Required]
        [Column("CODE")]
        public string Code { get; set; } = null!;

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
