using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("INFO_REGION")]
    public class Region
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("FULL_NAME")]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Column("SHORT_NAME")]
        [MaxLength(300)]
        public string ShortName { get; set; }

        [Column("CODE")]
        [MaxLength(10)]
        public string Code { get; set; }
    }
}
