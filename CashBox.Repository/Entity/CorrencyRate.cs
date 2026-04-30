using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Entity
{
    [Table("INFO_CURRENCY_RATE")]
    public class CorrencyRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("CURRENCY_ID")]
        public int CurrencyId { get; set; }

        [Column("RATE")]
        public decimal Rate { get; set; } 

        [Column("DATE")]
        public DateTime Date { get; set; }

        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }
    }
}
