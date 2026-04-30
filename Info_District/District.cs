using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("INFO_DISTRICT")]
    public class District
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("FULL_NAME")]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Column("CODE")]
        [MaxLength(10)]
        public string Code { get; set; }

        [Column("REGION")]
        [MaxLength(300)]
        public string Region { get; set; }
    }
}
