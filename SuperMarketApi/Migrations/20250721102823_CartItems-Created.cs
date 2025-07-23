using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperMarketApi.Migrations
{
    /// <inheritdoc />
    public partial class CartItemsCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Products",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "brand",
                table: "Products",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "cTv9p4hwv50bJh9WUob4XpfuYU7+Xw+vfDTnyk9luso=");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 3,
                column: "Password",
                value: "R1Q4JO1IZ720I+SMoKaPJMoLeiVOkfP6iCLFlLtusxg=");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_UserID",
                table: "CartItems",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Products",
                newName: "brand");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "name");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 2,
                column: "Password",
                value: "d033e22ae348aeb5660fc2140aec35850c4da997");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "ID",
                keyValue: 3,
                column: "Password",
                value: "b362b9b8b6e9e5e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2");
        }
    }
}
