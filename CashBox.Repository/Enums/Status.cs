using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Enum
{
    [Table("ENUM_STATUS")]
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public int Id { get; set; }

        [Column("CODE")]
        [StringLength(50)]
        public string Code { get; set; } = null!; 
        
        [Column("SHORT_NAME")]
        [StringLength(150)]
        public string ShortName { get; set; } = null!;

        [Column("FULL_NAME")]
        [StringLength(300)]
        public string FullName { get; set; } = null!;        

    }
}
