using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCRUD_NCapas.Migrations
{
    /// <inheritdoc />
    public partial class v04AgregandoRelacionesentretablas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.DropIndex(
                name: "IX_MoviesActors_MovieId",
                table: "MoviesActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories");

            migrationBuilder.DropIndex(
                name: "IX_MovieCategories_MovieId",
                table: "MovieCategories");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MovieCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "MovieCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                columns: new[] { "MovieId", "ActorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories",
                columns: new[] { "MovieId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories");

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "MovieCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MovieId",
                table: "MovieCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoviesActors",
                table: "MoviesActors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCategories",
                table: "MovieCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MoviesActors_MovieId",
                table: "MoviesActors",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCategories_MovieId",
                table: "MovieCategories",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Categories_CategoryId",
                table: "MovieCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCategories_Movies_MovieId",
                table: "MovieCategories",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
