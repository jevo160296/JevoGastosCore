﻿// <auto-generated />
using System;
using JevoGastosCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JevoGastosCore.Migrations
{
    [DbContext(typeof(GastosContext))]
    partial class GastosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("JevoGastosCore.Model.Etiqueta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Total")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0.0);

                    b.HasKey("Id");

                    b.ToTable("Etiquetas");

                    b.HasDiscriminator<string>("Tipo").HasValue("Etiqueta");
                });

            modelBuilder.Entity("JevoGastosCore.Model.PayDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Day")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("PayDays");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EsMesFijo")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EtiquetaId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Meta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("REAL")
                        .HasDefaultValue(0.0);

                    b.Property<int>("Tipo")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EtiquetaId");

                    b.ToTable("Planes");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Transaccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .HasColumnType("TEXT");

                    b.Property<int>("DestinoId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrigenId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Valor")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("DestinoId");

                    b.HasIndex("OrigenId");

                    b.ToTable("Transacciones");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Credito", b =>
                {
                    b.HasBaseType("JevoGastosCore.Model.Etiqueta");

                    b.HasDiscriminator().HasValue("Credito");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Cuenta", b =>
                {
                    b.HasBaseType("JevoGastosCore.Model.Etiqueta");

                    b.Property<bool>("EsAhorro")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Cuenta");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Gasto", b =>
                {
                    b.HasBaseType("JevoGastosCore.Model.Etiqueta");

                    b.HasDiscriminator().HasValue("Gasto");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Ingreso", b =>
                {
                    b.HasBaseType("JevoGastosCore.Model.Etiqueta");

                    b.HasDiscriminator().HasValue("Ingreso");
                });

            modelBuilder.Entity("JevoGastosCore.Model.Plan", b =>
                {
                    b.HasOne("JevoGastosCore.Model.Etiqueta", "Etiqueta")
                        .WithMany("Planes")
                        .HasForeignKey("EtiquetaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("JevoGastosCore.Model.Transaccion", b =>
                {
                    b.HasOne("JevoGastosCore.Model.Etiqueta", "Destino")
                        .WithMany("TransaccionesDestino")
                        .HasForeignKey("DestinoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("JevoGastosCore.Model.Etiqueta", "Origen")
                        .WithMany("TransaccionesOrigen")
                        .HasForeignKey("OrigenId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
