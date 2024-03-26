using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendaval.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameMessageConnectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "SenderConnectionId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Message",
                newName: "ReceiverConnectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderConnectionId",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverConnectionId",
                table: "Message",
                newName: "ReceiverId");
        }
    }
}
