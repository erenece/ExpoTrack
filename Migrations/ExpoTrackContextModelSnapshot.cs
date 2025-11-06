using System;
using ExpoTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ExpoTrack.Migrations
{
    // Veritabanı şemasının son durumunu temsil eden snapshot sınıfı
    [DbContext(typeof(ExpoTrackContext))]
    partial class ExpoTrackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618

            // Genel yapılandırmalar
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            // Firma-Fuar ilişkileri (Many-to-Many ara tablo)
            modelBuilder.Entity("ExpoTrack.Models.FirmaFuarIliskileri", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("FirmaId")
                    .HasColumnType("int");

                b.Property<int>("FuarId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("FirmaId");
                b.HasIndex("FuarId");

                b.ToTable("FirmaFuarIliskileri");
            });

            // Firmalar tablosu
            modelBuilder.Entity("ExpoTrack.Models.Firmalar", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Email")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("FirmaAdi")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("IsDurumu")
                    .HasColumnType("nvarchar(max)");

                // Bu alan artık kaldırılmalı → Model'den ve Migration'dan sonra otomatik silinecek
                b.Property<string>("KatildigiFuarlar")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Telefon")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("WebSitesi")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Yetkili")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Firmalar");
            });

            // Fuarlar tablosu
            modelBuilder.Entity("ExpoTrack.Models.Fuarlar", b =>
            {
                b.Property<int>("FuarId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FuarId"));

                b.Property<string>("Adi")
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<DateTime?>("BaslangicTarihi")
                    .HasColumnType("datetime2");

                b.Property<DateTime?>("BitisTarihi")
                    .HasColumnType("datetime2");

                b.Property<string>("Sehir")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Yer")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("FuarId");

                b.ToTable("Fuarlar");
            });

            // Kullanicilar tablosu
            modelBuilder.Entity("ExpoTrack.Models.Kullanicilar", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Ad")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Email")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<DateTime?>("OlusturulmaTarihi")
                    .HasColumnType("datetime2");

                b.Property<string>("Rol")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Sifre")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Soyad")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Kullanicilar");
            });

            // İlişkiler: Firma ↔️ Fuar
            modelBuilder.Entity("ExpoTrack.Models.FirmaFuarIliskileri", b =>
            {
                b.HasOne("ExpoTrack.Models.Firmalar", "Firma")
                    .WithMany("FuarIliskileri")
                    .HasForeignKey("FirmaId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ExpoTrack.Models.Fuarlar", "Fuar")
                    .WithMany("FirmaFuarIliskileri")
                    .HasForeignKey("FuarId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Firma");
                b.Navigation("Fuar");
            });

            // Navigation Property tanımlamaları
            modelBuilder.Entity("ExpoTrack.Models.Firmalar", b =>
            {
                b.Navigation("FuarIliskileri");
            });

            modelBuilder.Entity("ExpoTrack.Models.Fuarlar", b =>
            {
                b.Navigation("FirmaFuarIliskileri");
            });

#pragma warning restore 612, 618
        }
    }
}
