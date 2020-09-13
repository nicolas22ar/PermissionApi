using API.Test.Domain;
using Microsoft.EntityFrameworkCore;
 
namespace API.Test.Infrastructure.Concrete.DbContexts
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }
 
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionType> PermissionType { get; set; }
        
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Permission>(entity =>
            {

                entity.HasOne(x => x.Type)
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey(x => x.TypeId);

                entity.Property(x => x.PermissionDate)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(x => x.EmployeeFirstName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

                entity.Property(x => x.EmployeeLastName)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

            });

            builder.Entity<PermissionType>(entity => {
               
               entity.Property(x => x.Description)
                    .IsRequired()
                    .HasColumnType("varchar(255)");

            });
        }
    }
}