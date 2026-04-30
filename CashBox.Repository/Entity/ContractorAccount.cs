using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    [Table("HL_CONTRACTOR_ACCOUNT")]
    public class ContractorAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("CODE")]
        [StringLength(27)]
        public string Code { get; set; } = null!;

        [Column("CONTRACTOR_ID")]
        public int ContractorId { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public Contractor Contractor { get; set; } = null!;

        [Column("FULL_NAME")]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }
    }
}
