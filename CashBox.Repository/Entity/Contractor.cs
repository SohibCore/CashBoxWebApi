
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("HL_CONTRACTOR")]
    public class Contractor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("INN")]
        [StringLength(9)]
        public string Inn { get; set; } = null!;

        [Column("PINFL")]
        [StringLength(14)]
        public string Pinfl { get; set; } = null!;

        [Column("SHORT_NAME")]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Column("FULL_NAME")]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Column("REGION_ID")]
        public int RegionId { get; set; }
        [ForeignKey(nameof(RegionId))]
        public Region Region { get; set; } = null!;

        [Column("DISTRICT_ID")]
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; } = null!;

        [Column("CREATE_USER_ID")]
        public int CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }
    }
}
