using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Courses.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "instructor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    bio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image_url = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    name_first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name_last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_instructor", x => x.id);
                });

            migrationBuilder.Sql(
                """
                alter table "courses"
                add constraint "fk_course_instructor_instructor_id"
                foreign key ("instructor_id") references "instructor"("id")
                on delete restrict
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                alter table "courses"
                drop constraint "fk_course_instructor_instructor_id"
                """);
            migrationBuilder.DropTable(
                name: "instructor");
            
        }
    }
}
