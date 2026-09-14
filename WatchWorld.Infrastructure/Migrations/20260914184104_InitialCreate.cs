using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WatchWorld.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveBorrows",
                columns: table => new
                {
                    ListingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveBorrows", x => new { x.ListingId, x.BorrowId });
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BorrowedFromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOwnsWatches",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOwnsWatches", x => new { x.UserId, x.WatchId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchBorrows",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndividualWatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchBorrows", x => new { x.UserId, x.IndividualWatchId });
                });

            migrationBuilder.CreateTable(
                name: "Watchlist",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseSize = table.Column<int>(type: "int", nullable: false),
                    CaseShapeEnum = table.Column<int>(type: "int", nullable: false),
                    CaseMaterialEnum = table.Column<int>(type: "int", nullable: false),
                    MovementTypeEnum = table.Column<int>(type: "int", nullable: false),
                    Style = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GenderEnum = table.Column<int>(type: "int", nullable: false),
                    ReleaseYear = table.Column<DateOnly>(type: "date", nullable: false),
                    BraceletTypeEnum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatedToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingAmount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRatings_Users_RatedByUserId",
                        column: x => x.RatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRatings_Users_RatedToUserId",
                        column: x => x.RatedToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IndividualWatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WearGrade = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualWatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualWatches_Watchlist_WatchesId",
                        column: x => x.WatchesId,
                        principalTable: "Watchlist",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HighResImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    IndividualWatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WatchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighResImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HighResImages_IndividualWatches_IndividualWatchId",
                        column: x => x.IndividualWatchId,
                        principalTable: "IndividualWatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HighResImages_Watchlist_WatchesId",
                        column: x => x.WatchesId,
                        principalTable: "Watchlist",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IndividualWatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_IndividualWatches_IndividualWatchId",
                        column: x => x.IndividualWatchId,
                        principalTable: "IndividualWatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HighResImages_IndividualWatchId",
                table: "HighResImages",
                column: "IndividualWatchId");

            migrationBuilder.CreateIndex(
                name: "IX_HighResImages_WatchesId",
                table: "HighResImages",
                column: "WatchesId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualWatches_WatchesId",
                table: "IndividualWatches",
                column: "WatchesId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_IndividualWatchId",
                table: "Listings",
                column: "IndividualWatchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_RatedByUserId",
                table: "UserRatings",
                column: "RatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRatings_RatedToUserId",
                table: "UserRatings",
                column: "RatedToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveBorrows");

            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "HighResImages");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "UserOwnsWatches");

            migrationBuilder.DropTable(
                name: "UserRatings");

            migrationBuilder.DropTable(
                name: "WatchBorrows");

            migrationBuilder.DropTable(
                name: "IndividualWatches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Watchlist");
        }
    }
}
