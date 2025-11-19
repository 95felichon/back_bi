using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_app_par.models;
using Microsoft.EntityFrameworkCore;

namespace back_app_par.data
{
    public class appContext : DbContext
    {
        public appContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<usuario> usuario { get; set; }
        public DbSet<roles> roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<usuario>(x =>
            {
                x.HasKey(x => x.Id);

                x.Property(x => x.nombre)
                .IsRequired()
                .HasMaxLength(25);

                x.Property(x => x.passwordHash)
                .IsRequired();

                x.HasOne<roles>()
                .WithMany()
                .HasForeignKey(x => x.idRol)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<roles>(x =>
            {
                x.HasKey(x => x.id);

                x.Property(x => x.descripcion)
                .IsRequired()
                .HasMaxLength(20);
            });
        }

        protected appContext()
        {
        }
    }
}