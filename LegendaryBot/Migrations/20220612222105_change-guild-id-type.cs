using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegendaryBot.Migrations
{
    public partial class changeguildidtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactions_Guilds_GuildId1",
                table: "MessageReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageResponses_Guilds_GuildId1",
                table: "MessageResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageResponses_GuildId1",
                table: "MessageResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageReactions_GuildId1",
                table: "MessageReactions");

            migrationBuilder.DropColumn(
                name: "GuildId1",
                table: "MessageResponses");

            migrationBuilder.DropColumn(
                name: "GuildId1",
                table: "MessageReactions");

            migrationBuilder.CreateIndex(
                name: "IX_MessageResponses_GuildId",
                table: "MessageResponses",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_GuildId",
                table: "MessageReactions",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactions_Guilds_GuildId",
                table: "MessageReactions",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageResponses_Guilds_GuildId",
                table: "MessageResponses",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReactions_Guilds_GuildId",
                table: "MessageReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageResponses_Guilds_GuildId",
                table: "MessageResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageResponses_GuildId",
                table: "MessageResponses");

            migrationBuilder.DropIndex(
                name: "IX_MessageReactions_GuildId",
                table: "MessageReactions");

            migrationBuilder.AddColumn<int>(
                name: "GuildId1",
                table: "MessageResponses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuildId1",
                table: "MessageReactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MessageResponses_GuildId1",
                table: "MessageResponses",
                column: "GuildId1");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReactions_GuildId1",
                table: "MessageReactions",
                column: "GuildId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReactions_Guilds_GuildId1",
                table: "MessageReactions",
                column: "GuildId1",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageResponses_Guilds_GuildId1",
                table: "MessageResponses",
                column: "GuildId1",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
