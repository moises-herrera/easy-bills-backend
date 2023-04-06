using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasyBills.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class RenameUserColumnName : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Name",
            table: "Users",
            newName: "FirstName");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "FirstName",
            table: "Users",
            newName: "Name");
    }
}
