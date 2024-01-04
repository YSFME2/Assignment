using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPercent",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData("AspNetRoles",
                columns: ["Id", "Name", "NormalizedName", "ConcurrencyStamp"],
                values: [Guid.NewGuid().ToString(), "Admin", "ADMIN", Guid.NewGuid().ToString()]);

            migrationBuilder.InsertData("AspNetRoles",
                columns: ["Id", "Name", "NormalizedName", "ConcurrencyStamp"],
                values: [Guid.NewGuid().ToString(), "User", "USER", Guid.NewGuid().ToString()]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiscountPercent",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.Sql("DELETE FROM AspNetRoles");
        }
    }
}
