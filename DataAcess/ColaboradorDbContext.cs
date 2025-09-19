using crud_maui.Models;
using crud_maui.Utils;
using Microsoft.EntityFrameworkCore;

namespace crud_maui.DataAcess
{
    public class ColaboradorDbContext : DbContext
    {
        public DbSet<Colaborador> Colaboradores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conectionDB = $"Filename={ConnectDB.DevolverRota("colaboradores.db")}";
            optionsBuilder.UseSqlite(conectionDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.HasKey(col => col.IdColaborador);
                entity.Property(col => col.IdColaborador).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}