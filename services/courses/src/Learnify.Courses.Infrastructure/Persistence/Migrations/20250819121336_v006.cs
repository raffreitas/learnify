using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learnify.Courses.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class v006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
