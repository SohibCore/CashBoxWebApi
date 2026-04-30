using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("INFO_ORGANIZATION")]
    public class Organization
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("INN")]
        [MaxLength(10)]
        public string Inn { get; set; }

        [Column("FULL_NAME")]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Column("SHORT_NAME")]
        [MaxLength(300)]
        public string ShortName { get; set; }

        [Column("REGION_ID")]
        public int RegionId { get; set; }

        [Column("REGION")]
        [MaxLength(300)]
        public string Region { get; set; }

        [Column("DISTRICT")]
        [MaxLength(300)]
        public string District { get; set; }
    }
}
