using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Products.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Products.Repository
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Products table
            builder.Entity<Product>().HasKey(p => p.Id).HasName("PrimaryKey_Id");
            builder.Entity<Product>().Property(p => p.Id).IsRequired();
            builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().Property(p => p.DeliveryPrice).HasColumnType("decimal(18,2)");
            builder.Entity<Product>().ToTable($"{nameof(Products)}");

            // ProductOptions table
            builder.Entity<ProductOption>().ToTable($"{nameof(ProductOptions)}");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditEntities();
            return await base.SaveChangesAsync();
        }

        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                }
                else
                {
                    entity.UpdatedDate = now;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }
            }
        }
    }
}
