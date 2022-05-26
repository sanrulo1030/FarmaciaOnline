using FarmaciaOnline.Web.Data.Entities;
using FarmaciaOnline.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace FarmaciaOnline.Web.Data

{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Laboratory> Laboratories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
            .HasIndex(t => t.Name)
             .IsUnique();
             
             modelBuilder.Entity<Repository>(rep => 
            { 
                rep.HasIndex("Name").IsUnique(); 
                rep.HasMany(r => r.Medicines).WithOne(m => m.Repository).OnDelete(DeleteBehavior.Cascade); 
            }); 
            modelBuilder.Entity<Medicine>(med => 
            { 
                med.HasIndex("Name", "RepositoryId").IsUnique(); 
                med.HasOne(m => m.Repository).WithMany(r => r.Medicines).OnDelete(DeleteBehavior.Cascade); 
            }); 
            modelBuilder.Entity<Laboratory>(lab => 
            { 
                lab.HasIndex("Name", "MedicineId").IsUnique(); 
                lab.HasOne(r => r.Medicine).WithMany(m => m.Laboratories).OnDelete(DeleteBehavior.Cascade); 
            }); 
            modelBuilder.Entity<Product>()
            .HasIndex(t => t.Name)
            .IsUnique();

        }

    }
}


