using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendaval.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConversationsFromChatUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatUserConversation");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "ChatUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_ConversationId",
                table: "ChatUsers",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatUsers_Conversations_ConversationId",
                table: "ChatUsers",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatUsers_Conversations_ConversationId",
                table: "ChatUsers");

            migrationBuilder.DropIndex(
                name: "IX_ChatUsers_ConversationId",
                table: "ChatUsers");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "ChatUsers");

            migrationBuilder.CreateTable(
                name: "ChatUserConversation",
                columns: table => new
                {
                    ConversationsId = table.Column<int>(type: "integer", nullable: false),
                    ParticipantsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUserConversation", x => new { x.ConversationsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_ChatUserConversation_ChatUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "ChatUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUserConversation_Conversations_ConversationsId",
                        column: x => x.ConversationsId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUserConversation_ParticipantsId",
                table: "ChatUserConversation",
                column: "ParticipantsId");
        }
    }
}
