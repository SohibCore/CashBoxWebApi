
namespace RepositoryLayer.Entity
{
    [Table("HL_CONTRACTOR")]
    public class Contractor
    {
        [Key]  // PRIMARY KEY ligini bildirish
        [Column("ID")]
        public int Id { get; set; }

        [Column("INN")]
        [MaxLength(9)]
        public string Inn { get; set; }

        [Column("PINFL")]
        [MaxLength(14)]
        public string Pinfl { get; set; }

        [Column("SHORT_NAME")]
        [MaxLength(300)]
        public string ShortName { get; set; }

        [Column("FULL_NAME")]
        [MaxLength(500)]
        public string FullName { get; set; }

        [ForeignKey("Region")] 
        [Column("REGION_ID")]
        public int RegionId { get; set; }

        [ForeignKey("District")] 
        [Column("DISTRICT_ID")]
        public int DistrictId { get; set; }
    }
}
