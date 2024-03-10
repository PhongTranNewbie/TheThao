using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheThao.Migrations
{
    public partial class AddIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Lệnh tạo bảng LoaiSanPham đã bị loại bỏ
            // migrationBuilder.CreateTable(
            //     name: "LoaiSanPham",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         TenLoaiSanPham = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_LoaiSanPham", x => x.Id);
            //     });

            migrationBuilder.CreateTable(
                name: "NhaCungCap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaCungCap = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DiaChi = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    SDT = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaCungCap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    NgayThangNamSinh = table.Column<DateTime>(type: "date", nullable: true),
                    DiaChi = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Username = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Role = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    ChiTiet = table.Column<string>(type: "text", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    HinhAnh = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TheLoaiId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SanPham_LoaiSanPham",
                        column: x => x.TheLoaiId,
                        principalTable: "LoaiSanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiaChi = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Ngay = table.Column<DateTime>(type: "date", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Shipping__UserId__45F365D3",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KhoHang",
                columns: table => new
                {
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__KhoHang__FAC7442D6BC76C32", x => x.MaSanPham);
                    table.ForeignKey(
                        name: "FK__KhoHang__MaSanPh__4CA06362",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NhapHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSanPham = table.Column<int>(type: "int", nullable: true),
                    SoLuongNhap = table.Column<int>(type: "int", nullable: true),
                    NgayNhap = table.Column<DateTime>(type: "date", nullable: true),
                    MaNhaCungCap = table.Column<int>(type: "int", nullable: true),
                    GiaNhap = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhapHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK__NhapHang__UserId__412EB0B6",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NhapHang_NhaCungCap",
                        column: x => x.MaNhaCungCap,
                        principalTable: "NhaCungCap",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NhapHang_SanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DonHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ngay = table.Column<DateTime>(type: "date", nullable: true),
                    TrangThai = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ShippingId = table.Column<int>(type: "int", nullable: true),
                    DiaChi = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DonHang__Shippin__5535A963",
                        column: x => x.ShippingId,
                        principalTable: "Shipping",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__DonHang__UserId__5441852A",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGiaoDichUser = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MaGiaoDichMerchant = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MaDonHang = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GioHang_DonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GioHang_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdSanPham = table.Column<int>(type: "int", nullable: true),
                    IdGioHang = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Gia = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ChiTietGi__IdGio__5070F446",
                        column: x => x.IdGioHang,
                        principalTable: "GioHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__ChiTietGi__IdSan__4F7CD00D",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ngay = table.Column<DateTime>(type: "date", nullable: true),
                    PhuongPhapThanhToan = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MaGioHang = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ThanhToan__MaGio__3C69FB99",
                        column: x => x.MaGioHang,
                        principalTable: "GioHang",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThanhToan_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_IdGioHang",
                table: "ChiTietGioHang",
                column: "IdGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHang_IdSanPham",
                table: "ChiTietGioHang",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_ShippingId",
                table: "DonHang",
                column: "ShippingId");

            migrationBuilder.CreateIndex(
                name: "IX_DonHang_UserId",
                table: "DonHang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_MaDonHang",
                table: "GioHang",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_UserId",
                table: "GioHang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_MaNhaCungCap",
                table: "NhapHang",
                column: "MaNhaCungCap");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_MaSanPham",
                table: "NhapHang",
                column: "MaSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_UserId",
                table: "NhapHang",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_TheLoaiId",
                table: "SanPham",
                column: "TheLoaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_UserId",
                table: "Shipping",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_MaGioHang",
                table: "ThanhToan",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_UserId",
                table: "ThanhToan",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietGioHang");

            migrationBuilder.DropTable(
                name: "KhoHang");

            migrationBuilder.DropTable(
                name: "NhapHang");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "NhaCungCap");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "LoaiSanPham");

            migrationBuilder.DropTable(
                name: "DonHang");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
