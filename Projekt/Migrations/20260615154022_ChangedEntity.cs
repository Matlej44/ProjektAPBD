using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekt.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoftwareVersions_SoftwareId",
                table: "SoftwareVersions");

            migrationBuilder.DropColumn(
                name: "PeriodEndDate",
                table: "SubscriptionPayments");

            migrationBuilder.DropColumn(
                name: "PeriodStartDate",
                table: "SubscriptionPayments");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_SoftwareId_Version",
                table: "SoftwareVersions",
                columns: new[] { "SoftwareId", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoftwareVersions_SoftwareId_Version",
                table: "SoftwareVersions");

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodEndDate",
                table: "SubscriptionPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodStartDate",
                table: "SubscriptionPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SubscriptionPayments",
                keyColumn: "SubscriptionPaymentId",
                keyValue: 1,
                columns: new[] { "PeriodEndDate", "PeriodStartDate" },
                values: new object[] { new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "SubscriptionPayments",
                keyColumn: "SubscriptionPaymentId",
                keyValue: 2,
                columns: new[] { "PeriodEndDate", "PeriodStartDate" },
                values: new object[] { new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersions_SoftwareId",
                table: "SoftwareVersions",
                column: "SoftwareId");
        }
    }
}
