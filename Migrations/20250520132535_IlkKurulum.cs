using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpoTrack.Migrations
{
    /// <summary>
    /// İlk veritabanı kurulumunu yapan migration dosyası.
    /// </summary>
    public partial class IlkKurulum : Migration
    {
        // Veritabanı şeması bu metodla oluşturulur
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Firmalar tablosu oluşturuluyor
            migrationBuilder.CreateTable(
                name: "Firmalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebSitesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yetkili = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDurumu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KatildigiFuarlar = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firmalar", x => x.Id);
                });

            // Fuarlar tablosu oluşturuluyor
            migrationBuilder.CreateTable(
                name: "Fuarlar",
                columns: table => new
                {
                    FuarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sehir = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Yer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuarlar", x => x.FuarId);
                });

            // Kullanicilar tablosu oluşturuluyor
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Soyad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Sifre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            // Firma-Fuar ilişki tablosu oluşturuluyor (Many-to-Many için)
            migrationBuilder.CreateTable(
                name: "FirmaFuarIliskileri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuarId = table.Column<int>(type: "int", nullable: false),
                    FirmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmaFuarIliskileri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirmaFuarIliskileri_Firmalar_FirmaId",
                        column: x => x.FirmaId,
                        principalTable: "Firmalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirmaFuarIliskileri_Fuarlar_FuarId",
                        column: x => x.FuarId,
                        principalTable: "Fuarlar",
                        principalColumn: "FuarId",
                        onDelete: ReferentialAction.Cascade);
                });

            // Index'ler (performans için)
            migrationBuilder.CreateIndex(
                name: "IX_FirmaFuarIliskileri_FirmaId",
                table: "FirmaFuarIliskileri",
                column: "FirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmaFuarIliskileri_FuarId",
                table: "FirmaFuarIliskileri",
                column: "FuarId");
        }

        // Veritabanı aşağıya çevrilmek istendiğinde bu metot çalışır (geri alma işlemi)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "FirmaFuarIliskileri");
            migrationBuilder.DropTable(name: "Kullanicilar");
            migrationBuilder.DropTable(name: "Firmalar");
            migrationBuilder.DropTable(name: "Fuarlar");
        }
    }
}
