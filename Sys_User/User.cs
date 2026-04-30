using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("SYS_USER")]
    public class User
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_NAME")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Column("FULL_NAME")]
        [MaxLength(500)]
        public string FullName { get; set; }

        [Column("SHORT_NAME")]
        [MaxLength(300)]
        public string ShortName { get; set; }

        [Column("PINFL")]
        [MaxLength(14)]
        public int Pinfl { get; set; }

        [Column("PHONE_NUMBER")]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Column("ADRESS")]
        [MaxLength(300)]
        public string Adress { get; set; }

        [Column("ORGANIZATION_ID")]
        public string OrganizationId { get; set; }

        [Column("DATE_OF_BIRTH")]
        public DateTime DateOfBirth { get; set; }

        [Column("PASSPORT_SERIES")]
        [MaxLength(9)]
        public string PassportSeries { get; set; }
        //[ForeignKey]
        public Organization Organization { get; set; }
    }
}
