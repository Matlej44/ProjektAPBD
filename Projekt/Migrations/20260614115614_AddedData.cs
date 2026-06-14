using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Projekt.Migrations
{
    /// <inheritdoc />
    public partial class AddedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareDiscounts_Discounts_DiscountsDiscountId",
                table: "SoftwareDiscounts");

            migrationBuilder.RenameColumn(
                name: "DiscountsDiscountId",
                table: "SoftwareDiscounts",
                newName: "DiscountId");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "jan.kowalski@email.com", "500-100-200" },
                    { 2, "anna.nowak@email.com", "600-200-300" },
                    { 3, "kontakt@techcorp.pl", "22-300-400-500" },
                    { 4, "biuro@edusoft.pl", "12-400-500-600" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "DiscountPercent", "EndDate", "IsRepetitive", "Name", "StartDate" },
                values: new object[,]
                {
                    { 1, 10m, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Black Friday Discount", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 15m, new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Summer Sale", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "SoftwareId", "ClientId", "Name", "SoftwareTypeId" },
                values: new object[,]
                {
                    { 1, null, "FinManager Pro", 1 },
                    { 2, null, "EduLearn", 2 }
                });

            migrationBuilder.InsertData(
                table: "ClientCompanies",
                columns: new[] { "ClientId", "Address", "CompanyName", "KRS" },
                values: new object[,]
                {
                    { 3, "ul. Przemysłowa 10, Warszawa", "TechCorp Sp. z o.o.", "0000123456" },
                    { 4, "ul. Akademicka 3, Kraków", "EduSoft S.A.", "0000654321" }
                });

            migrationBuilder.InsertData(
                table: "ClientPerson",
                columns: new[] { "ClientId", "Name", "Pesel", "Surname" },
                values: new object[,]
                {
                    { 1, "Jan", "90010112345", "Kowalski" },
                    { 2, "Anna", "85052267890", "Nowak" }
                });

            migrationBuilder.InsertData(
                table: "SoftwareDiscounts",
                columns: new[] { "DiscountId", "SoftwareId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareVersions",
                columns: new[] { "SoftwareVersionId", "ReleaseDate", "SoftwareId", "Version", "YearlyPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1.0.0", 5000m },
                    { 2, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "2.0.0", 6000m },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "3.0.0", 7000m },
                    { 4, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "1.0.0", 3000m },
                    { 5, new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "2.0.0", 4000m }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionOffers",
                columns: new[] { "SubscriptionOfferId", "Name", "Price", "RenewalPeriod", "SoftwareId" },
                values: new object[,]
                {
                    { 1, "FinManager Monthly", 500m, 1, 1 },
                    { 2, "FinManager Yearly", 5000m, 12, 1 },
                    { 3, "EduLearn Monthly", 300m, 1, 2 },
                    { 4, "EduLearn Yearly", 3000m, 12, 2 }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "ContractId", "AdditionalSupportYears", "ClientId", "CreatedAt", "EndDate", "IsActive", "SoftwareId", "SoftwareVersionId", "StartDate", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8000m },
                    { 2, 0, 3, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 5, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000m }
                });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "SubscriptionId", "ClientId", "EndDate", "StartDate", "SubscriptionOfferId" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 2, 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "ClientId", "ContractId", "Date" },
                values: new object[,]
                {
                    { 1, 4000m, 1, 1, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 4000m, 1, 1, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 4000m, 3, 2, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPayments",
                columns: new[] { "SubscriptionPaymentId", "Amount", "PaymentDate", "PeriodEndDate", "PeriodStartDate", "SubscriptionId" },
                values: new object[,]
                {
                    { 1, 300m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 5000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareDiscounts_Discounts_DiscountId",
                table: "SoftwareDiscounts",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareDiscounts_Discounts_DiscountId",
                table: "SoftwareDiscounts");

            migrationBuilder.DeleteData(
                table: "ClientCompanies",
                keyColumn: "ClientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ClientCompanies",
                keyColumn: "ClientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ClientPerson",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ClientPerson",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SoftwareDiscounts",
                keyColumns: new[] { "DiscountId", "SoftwareId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "SoftwareDiscounts",
                keyColumns: new[] { "DiscountId", "SoftwareId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "SoftwareVersionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "SoftwareVersionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "SoftwareVersionId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubscriptionOffers",
                keyColumn: "SubscriptionOfferId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubscriptionOffers",
                keyColumn: "SubscriptionOfferId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SubscriptionPayments",
                keyColumn: "SubscriptionPaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SubscriptionPayments",
                keyColumn: "SubscriptionPaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "ContractId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "ContractId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subscriptions",
                keyColumn: "SubscriptionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "SoftwareVersionId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SoftwareVersions",
                keyColumn: "SoftwareVersionId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SubscriptionOffers",
                keyColumn: "SubscriptionOfferId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SubscriptionOffers",
                keyColumn: "SubscriptionOfferId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "SoftwareId",
                keyValue: 2);

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "SoftwareDiscounts",
                newName: "DiscountsDiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareDiscounts_Discounts_DiscountsDiscountId",
                table: "SoftwareDiscounts",
                column: "DiscountsDiscountId",
                principalTable: "Discounts",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
