using FluentMigrator;

namespace Learnify.Identity.WebApi.Shared.Infrastructure.Persistence.Migrations.Versions;

[Migration(DatabaseVersions.TableUsers, "Create users table")]
public sealed class Version000001 : Migration
{
    public const string TableName = "users";

    public override void Up()
    {
        Create.Table(TableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("provider_key").AsString(255).NotNullable().Unique()
            .WithColumn("provider_type").AsString(50).NotNullable()
            .WithColumn("email").AsString(255).NotNullable().Unique()
            .WithColumn("first_name").AsString(100).NotNullable()
            .WithColumn("last_name").AsString(100).Nullable()
            .WithColumn("role").AsAnsiString().NotNullable()
            .WithColumn("is_active").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("picture").AsString(500).Nullable()
            .WithColumn("created_at").AsDateTime2().NotNullable()
            .WithColumn("updated_at").AsDateTime2().NotNullable();
    }

    public override void Down()
    {
        Delete.Table(TableName);
    }
}
