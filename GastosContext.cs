using JevoGastosCore.Model;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace JevoGastosCore
{
    public class GastosContext : DbContext
    {
        #region DataBaseTables
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        #endregion
        #region Configuration
        private static string dbname = "db.db";
        public string FolderPath { get; set; } = null;
        public string DbPath { get => Path.Combine(FolderPath, dbname); }
        #endregion

        public GastosContext() : base()
        {
            FolderPath = "C:/";
        }
        public GastosContext(string folderpath) : base()
        {
            FolderPath = folderpath;
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Etiqueta
            modelBuilder.Entity<Etiqueta>()
                .Property(p => p.Total)
                .HasDefaultValue(0);
            modelBuilder.Entity<Etiqueta>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Etiqueta>()
                .HasDiscriminator<string>("Tipo")
                .HasValue<Ingreso>("Ingreso")
                .HasValue<Cuenta>("Cuenta")
                .HasValue<Gasto>("Gasto");

            //Transaccion
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Origen)
                .WithMany(p => p.TransaccionesOrigen);
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Destino)
                .WithMany(p => p.TransaccionesDestino);
        }
    }
}
