using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheThao.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhapHang_SanPham",
                table: "NhapHang");

            migrationBuilder.DropIndex(
                name: "IX_NhapHang_MaSanPham",
                table: "NhapHang");

            migrationBuilder.DropColumn(
                name: "GiaNhap",
                table: "NhapHang");

            migrationBuilder.DropColumn(
                name: "MaSanPham",
                table: "NhapHang");

            migrationBuilder.RenameColumn(
                name: "SoLuongNhap",
                table: "NhapHang",
                newName: "SanPhamId");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "NhapHang",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChiTietNhapHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    MaNhapHang = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietNhapHangs", x => new { x.Id, x.MaSanPham, x.MaNhapHang });
                    table.ForeignKey(
                        name: "FK_ChiTietNhapHangs_NhapHang_MaNhapHang",
                        column: x => x.MaNhapHang,
                        principalTable: "NhapHang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietNhapHangs_SanPham_MaSanPham",
                        column: x => x.MaSanPham,
                        principalTable: "SanPham",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_SanPhamId",
                table: "NhapHang",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhapHangs_MaNhapHang",
                table: "ChiTietNhapHangs",
                column: "MaNhapHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietNhapHangs_MaSanPham",
                table: "ChiTietNhapHangs",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_NhapHang_SanPham_SanPhamId",
                table: "NhapHang",
                column: "SanPhamId",
                principalTable: "SanPham",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhapHang_SanPham_SanPhamId",
                table: "NhapHang");

            migrationBuilder.DropTable(
                name: "ChiTietNhapHangs");

            migrationBuilder.DropIndex(
                name: "IX_NhapHang_SanPhamId",
                table: "NhapHang");

            migrationBuilder.RenameColumn(
                name: "SanPhamId",
                table: "NhapHang",
                newName: "SoLuongNhap");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "NhapHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GiaNhap",
                table: "NhapHang",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaSanPham",
                table: "NhapHang",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NhapHang_MaSanPham",
                table: "NhapHang",
                column: "MaSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_NhapHang_SanPham",
                table: "NhapHang",
                column: "MaSanPham",
                principalTable: "SanPham",
                principalColumn: "Id");
        }
    }
}
