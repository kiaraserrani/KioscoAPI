
using Kiosco.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kiosco.Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.Descripcion)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(p => p.Precio)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(p => p.Stock)
                    .IsRequired();

                entity.Property(p => p.Categoria)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.Activo)
                    .HasDefaultValue(true)
                    .IsRequired();
            });
        }
    }
}
