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
        public DbSet<Credito> Creditos { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<Plan> Planes { get; set; }
        public DbSet<PayDay> PayDays { get; set; }
        #endregion
        #region Configuration
        private string dbname = "db.db";
        public string DbName
        {
            get 
            {
                return dbname;
            }
            set
            {
                if (value!=dbname)
                {
                    dbname = value;
                    this.Database.GetDbConnection().ConnectionString=$"Filename={DbPath}";
                }
            }
        }
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
        public GastosContext(string folderpath,string dbname)
        {
            FolderPath = folderpath;
            this.dbname = dbname;
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
                .HasValue<Gasto>("Gasto")
                .HasValue<Credito>("Credito");
            //Transaccion
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Origen)
                .WithMany(p => p.TransaccionesOrigen)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Destino)
                .WithMany(p => p.TransaccionesDestino)
                .OnDelete(DeleteBehavior.Restrict);
            //Plan
            modelBuilder.Entity<Plan>()
                .Property(p => p.Meta)
                .HasDefaultValue(0);
            modelBuilder.Entity<Plan>()
                .Property(p => p.Tipo)
                .IsRequired();
            modelBuilder.Entity<Plan>()
                .HasOne(p => p.Etiqueta)
                .WithMany(p => p.Planes)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Plan>()
                .Ignore(p => p.MetaMensual)
                .Ignore(p => p.MetaDiaria)
                .Ignore(p => p.ValorActual)
                .Ignore(p => p.Falta)
                .Ignore(p=>p.UpdateTracker);
            //General
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
        }
    }
}
