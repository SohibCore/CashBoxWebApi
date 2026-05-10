using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("INFO_ORGANIZATION")]
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; } 

        [Column("INN")]
        [StringLength(10)]
        public string Inn { get; set; } = null!;    

        [Column("FULL_NAME")]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Column("SHORT_NAME")]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Column("REGION_ID")]
        public int RegionId { get; set; } 
        [ForeignKey(nameof(RegionId))]
        public Region? Region { get; set; }

        [Column("DISTRICT")]
        [StringLength(300)]
        public string District { get; set; } = null!;

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
