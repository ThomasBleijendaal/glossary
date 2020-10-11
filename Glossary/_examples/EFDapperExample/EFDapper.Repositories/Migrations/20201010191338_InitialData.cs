using Microsoft.EntityFrameworkCore.Migrations;

namespace EFDapper.Repositories.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Test Person 1" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "City", "Country", "PersonId", "Street", "Zipcode" },
                values: new object[,]
                {
                    { 1, "City 1", "Country 1", 1, "Street 123", "ZIP123" },
                    { 2, "City 2", "Country 1", 1, "Street 1234", "ZIP1234" }
                });

            migrationBuilder.InsertData(
                table: "EmailAddresses",
                columns: new[] { "Id", "Active", "Emailaddress", "PersonId" },
                values: new object[,]
                {
                    { 1, true, "emailaddress1@example.com", 1 },
                    { 2, true, "emailaddress2@example.com", 1 },
                    { 3, true, "emailaddress3@example.com", 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Addresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EmailAddresses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
