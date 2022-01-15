using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hydra.Server.Auth.Migrations
{
    public partial class VideoDbInitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    TrainerId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TrainerId = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    UploadedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UploadedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    VideoClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoGroups_VideoClasses_VideoClassId",
                        column: x => x.VideoClassId,
                        principalTable: "VideoClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoToPlaylist",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoToPlaylist", x => new { x.PlaylistId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VideoToPlaylist_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoToPlaylist_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistToGroup",
                columns: table => new
                {
                    PlaylistId = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistToGroup", x => new { x.PlaylistId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_PlaylistToGroup_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlaylistToGroup_VideoGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "VideoGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToGroup",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToGroup", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserToGroup_VideoGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "VideoGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistToGroup_GroupId",
                table: "PlaylistToGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToGroup_GroupId",
                table: "UserToGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoGroups_VideoClassId",
                table: "VideoGroups",
                column: "VideoClassId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoToPlaylist_VideoId",
                table: "VideoToPlaylist",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistToGroup");

            migrationBuilder.DropTable(
                name: "UserToGroup");

            migrationBuilder.DropTable(
                name: "VideoToPlaylist");

            migrationBuilder.DropTable(
                name: "VideoGroups");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "VideoClasses");
        }
    }
}
