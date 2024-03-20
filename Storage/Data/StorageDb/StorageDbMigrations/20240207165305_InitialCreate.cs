using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Storage.Data.StorageDb.StorageDbMigrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "StoragePlaces",
                columns: table => new
                {
                    StoragePlaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoragePlaces", x => x.StoragePlaceId);
                });

            migrationBuilder.CreateTable(
                name: "StoragePlacesCells",
                columns: table => new
                {
                    StoragePlaceCellId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoragePlacesCells", x => x.StoragePlaceCellId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    StoragePlaceId = table.Column<int>(type: "int", nullable: true),
                    StoragePlaceCellId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                    table.ForeignKey(
                        name: "FK_Locations_StoragePlacesCells_StoragePlaceCellId",
                        column: x => x.StoragePlaceCellId,
                        principalTable: "StoragePlacesCells",
                        principalColumn: "StoragePlaceCellId");
                    table.ForeignKey(
                        name: "FK_Locations_StoragePlaces_StoragePlaceId",
                        column: x => x.StoragePlaceId,
                        principalTable: "StoragePlaces",
                        principalColumn: "StoragePlaceId");
                });

            migrationBuilder.CreateTable(
                name: "StorageItems",
                columns: table => new
                {
                    StorageItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageItems", x => x.StorageItemId);
                    table.ForeignKey(
                        name: "FK_StorageItems_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RoomId",
                table: "Locations",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StoragePlaceCellId",
                table: "Locations",
                column: "StoragePlaceCellId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StoragePlaceId",
                table: "Locations",
                column: "StoragePlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_LocationId",
                table: "StorageItems",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageItems_UserId",
                table: "StorageItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageItems");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "StoragePlacesCells");

            migrationBuilder.DropTable(
                name: "StoragePlaces");
        }
    }
}
