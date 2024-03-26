using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendaval.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSenderAndReceiverUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "Message",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Message",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Message");
        }
    }
}
