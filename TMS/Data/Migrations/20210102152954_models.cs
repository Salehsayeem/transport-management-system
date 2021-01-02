using Microsoft.EntityFrameworkCore.Migrations;

namespace TMS.Data.Migrations
{
    public partial class models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteName = table.Column<string>(nullable: true),
                    RouteDistance = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInformation",
                columns: table => new
                {
                    VehicleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleNo = table.Column<string>(nullable: true),
                    SeatCapacity = table.Column<int>(nullable: false),
                    VehicleTypeId = table.Column<int>(nullable: false),
                    RouteId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInformation", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleInformation_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInformation_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "RouteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInformation_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformation_ApplicationUserId",
                table: "VehicleInformation",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformation_RouteId",
                table: "VehicleInformation",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformation_VehicleTypeId",
                table: "VehicleInformation",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleInformation");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
