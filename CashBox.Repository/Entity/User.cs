using CashBox.Repository.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entity
{
    [Table("SYS_USER")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("USER_NAME")]
        [StringLength(100)]
        public string UserName { get; set; } = null!;

        [Column("PASSWORD")]
        [StringLength(300)]
        public string Password { get; set; } = null!;

        //[Column("ROLE")]
        //[StringLength(50)]
        //public string Role { get; set; } = null!;

        [Column("FULL_NAME")]
        [StringLength(500)]
        public string? FullName { get; set; }

        [Column("SHORT_NAME")]
        [StringLength(300)]
        public string? ShortName { get; set; }

        [Column("PINFL")]
        [StringLength(14)]
        public string? Pinfl { get; set; }

        [Column("PHONE_NUMBER")]
        [StringLength(30)]
        public string? PhoneNumber { get; set; }

        [Column("ADDRESS")]
        [StringLength(300)]
        public string? Address { get; set; }

        [Column("ORGANIZATION_ID")]
        public int? OrganizationId { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public Organization? Organization { get; set; }

        [Column("DATE_OF_BIRTH")]
        [StringLength(10)]
        public string? DateOfBirth { get; set; }

        [Column("PASSPORT_SERIES")]
        [StringLength(9)]
        public string? PassportSeries { get; set; }

        [Column("EMAIL")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    = new List<UserRole>();
    }
}
