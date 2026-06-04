using CashBox.Repository.Entity;
using CashBox.Repository.Enum;
using Microsoft.EntityFrameworkCore;
using Repository.Entity;
using RepositoryLayer.Entity;

namespace Repository.Data
{
    public class AppDbContext : DbContext  //DbContext -> EF Core class
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }
        public DbSet<Contractor> Contractors { get; set; } //DB da Contractor class orqali Contractors table hosil qiladi
        public DbSet<ContractorAccount> ContractorAccounts { get; set; } //DB da ContractorAccount class orqali ContractorAccounts table hosil qiladi
        public DbSet<Currency> Correncys { get; set; } //DB da Currency class orqali Correncys table hosil qiladi
        public DbSet<CorrencyRate> CorrencyRates { get; set; } //DB da CorrencyRate class orqali CorrencyRates table hosil qiladi
        public DbSet<District> Districts { get; set; } //DB da District class orqali Districts table hosil qiladi
        public DbSet<Organization> Organizations { get; set; } //DB da Organization class orqali Organizations table hosil qiladi
        public DbSet<Region> Regions { get; set; } //DB da Region class orqali Regions table hosil qiladi
        public DbSet<User> Users { get; set; } //DB da User class orqali Users table hosil qiladi
        public DbSet<UserRole> UserRoles { get; set; } //DB da UserRole class orqali UserRoles table hosil qiladi
        public DbSet<Role> Roles { get; set; } //DB da Role class orqali Roles table hosil qiladi
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<IncomeDocument> IncomeDocuments { get; set; }
        public DbSet<IncomeDocumentTable> IncomeDocumentTables { get; set; }
        public DbSet<OutcomeDocument> OutcomeDocuments { get; set; }
        public DbSet<IncomeDocumentTable> OutcomeDocumentTables { get; set; }
        public DbSet<Status> Statuses{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
