using JevoGastosCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace JevoGastosCore.Context
{
    public class GastosContext:DbContext
    {
        #region DataBaseTables
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        public DbSet<TipoEtiqueta> TiposEtiquetas { get; set; }
        public DbSet<TipoTransaccion> TiposTransacciones { get; set; }
        #endregion
        #region Configuration
        private static string dbname = "db.db";
        public string FolderPath { get; set; } = null;
        public string DbPath { get => Path.Combine(FolderPath, dbname); }
        #endregion

        public GastosContext() : base()
        {
            this.FolderPath = "C:/";
        }
        public GastosContext(string folderpath):base()
        {
            this.FolderPath = folderpath;
            this.Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Filename={this.DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //Etiqueta
            modelBuilder.Entity<Etiqueta>()
                .Property(p => p.Id)
                .IsRequired();
            modelBuilder.Entity<Etiqueta>()
                .Property(p => p.Name)
                .IsRequired();
            modelBuilder.Entity<Etiqueta>()
                .HasOne(p => p.TipoEtiqueta)
                .WithMany(p => p.Etiquetas);
            //Transaccion
            modelBuilder.Entity<Transaccion>()
                .Property(p => p.TipoTransaccionId)
                .IsRequired();
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.TipoTransaccion)
                .WithMany(p => p.Transacciones);
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Origen)
                .WithMany(p => p.TransaccionesOrigen);
            modelBuilder.Entity<Transaccion>()
                .HasOne(p => p.Destino)
                .WithMany(p => p.TransaccionesDestino);
            //TipoEtiqueta
            modelBuilder.Entity<TipoEtiqueta>()
                .Property(p => p.Name)
                .IsRequired();
            //TipoTransaccion
            modelBuilder.Entity<TipoTransaccion>()
                .HasOne(p => p.TDestino)
                .WithMany(p => p.TTransaccionesDestino);
            modelBuilder.Entity<TipoTransaccion>()
                .HasOne(p => p.TOrigen)
                .WithMany(p => p.TTransaccionesOrigen);
        }
    }
}
