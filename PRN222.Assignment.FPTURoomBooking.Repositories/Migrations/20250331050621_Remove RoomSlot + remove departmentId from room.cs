using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN222.Assignment.FPTURoomBooking.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomSlotremovedepartmentIdfromroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Department_DepartmentId",
                table: "Room");

            migrationBuilder.DropTable(
                name: "RoomSlot");

            migrationBuilder.DropIndex(
                name: "IX_Room_DepartmentId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Room");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Room",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomSlot",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSlot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomSlot_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomSlot_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_DepartmentId",
                table: "Room",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomSlot_BookingId",
                table: "RoomSlot",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomSlot_RoomId",
                table: "RoomSlot",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Department_DepartmentId",
                table: "Room",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }
    }
}
