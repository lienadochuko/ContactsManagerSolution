using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsManager.Infastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecieveNewsLetter = table.Column<bool>(type: "bit", nullable: false),
                    NIN = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryID");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("09508eb3-57db-49ce-b368-0a00ffb75828"), "ZERIOBIA" },
                    { new Guid("1e00daea-f817-4375-9ec9-a6e6bd48bace"), "ONARIA" },
                    { new Guid("40db2e52-37ab-43c3-8b22-eb24a2084ded"), "ONSLOW" },
                    { new Guid("6e0a41b7-baf4-4f2b-bace-a654e87c664b"), "HOLDFAST" },
                    { new Guid("7680241d-34b8-4f39-b494-87c27831866c"), "ISREAL" },
                    { new Guid("84312db0-4672-407d-b212-87e550c74428"), "EIGHT-MAN-EMPIRE" },
                    { new Guid("9ce71166-8a69-481d-9c5c-2ea5aac9e73b"), "ALGERIA" },
                    { new Guid("a5386467-0411-44ca-90c8-e3a26b655b94"), "AUSTRALIA" },
                    { new Guid("a6939d16-ea43-442c-adee-68738a2b39cb"), "BENIN" },
                    { new Guid("bf9830c7-8e3d-485c-8fb1-9c7f9652fc75"), "BULGARIA" },
                    { new Guid("c3ee9030-141f-4b92-b1ac-df45a84a9046"), "BELGIUM" },
                    { new Guid("e801e3c0-7835-4760-9e0b-27078011a2e5"), "ETHOPIA" },
                    { new Guid("ee69e6b7-641c-4abb-80b6-1d3971edc904"), "ENGLAND" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DOB", "Email", "Gender", "NIN", "PersonName", "RecieveNewsLetter" },
                values: new object[,]
                {
                    { new Guid("012107df-862f-4f16-ba94-e5c16886f005"), "86 Rowland Avenue", new Guid("6e0a41b7-baf4-4f2b-bace-a654e87c664b"), new DateTime(2001, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "cpentelo1u@posterous.com", "Female", "SDBBBB-1B0", "Clarie Pentelo", true },
                    { new Guid("0378baa8-586b-4ec6-985d-78c2a80d47cc"), "87545 Village Green Hill", new Guid("c3ee9030-141f-4b92-b1ac-df45a84a9046"), new DateTime(2000, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "chansom2r@hugedomains.com", "Male", "SDBBBB-1B3", "Calhoun Hansom", true },
                    { new Guid("28d11936-9466-4a4b-b9c5-2f0a8e0cbde9"), "892 Nova Place", new Guid("a6939d16-ea43-442c-adee-68738a2b39cb"), new DateTime(1998, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "kmatisse1w@a8.net", "Male", "SDBBBB-1B2", "Keen Matisse", true },
                    { new Guid("29339209-63f5-492f-8459-754943c74abf"), "4 Bultman Junction", new Guid("40db2e52-37ab-43c3-8b22-eb24a2084ded"), new DateTime(1992, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "mgrotty1s@zdnet.com", "Female", "SDBBBB-1A6", "Maureen Grotty", true },
                    { new Guid("2a6d3738-9def-43ac-9279-0310edc7ceca"), "00098 Hanson Hill", new Guid("09508eb3-57db-49ce-b368-0a00ffb75828"), new DateTime(1995, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "balgar1r@altervista.org", "Male", "SDBBBB-1A5", "Blaine Algar", true },
                    { new Guid("89e5f445-d89f-4e12-94e0-5ad5b235d704"), "20 Johnson Point", new Guid("1e00daea-f817-4375-9ec9-a6e6bd48bace"), new DateTime(1999, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "rdawidowitz1q@goo.gl", "Female", "SDBBBB-1A4", "Rosalyn Dawidowitz", true },
                    { new Guid("a3b9833b-8a4d-43e9-8690-61e08df81a9a"), "60021 Westend Junction", new Guid("9ce71166-8a69-481d-9c5c-2ea5aac9e73b"), new DateTime(1992, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "mczajkowski1x@ning.com", "Male", "SDBBBB-1B4", "Mickey Czajkowski", true },
                    { new Guid("ac660a73-b0b7-4340-abc1-a914257a6189"), "84 Barnett Avenue", new Guid("e801e3c0-7835-4760-9e0b-27078011a2e5"), new DateTime(1993, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "llaurenceau1t@cafepress.com", "Female", "SDBBBB-1A7", "Lethia Laurenceau", true },
                    { new Guid("c03bbe45-9aeb-4d24-99e0-4743016ffce9"), "324 DavidsViews", new Guid("ee69e6b7-641c-4abb-80b6-1d3971edc904"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "davids.Grace@gmail.com", "Male", "SDBBBB-1", "Grace Davids", true },
                    { new Guid("c3abddbd-cf50-41d2-b6c4-cc7d5a750928"), "2 Washington Trail", new Guid("a5386467-0411-44ca-90c8-e3a26b655b94"), new DateTime(2001, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "whucknall1n@google.it", "Male", "SDBBBB-1A1", "Wheeler Hucknall", true },
                    { new Guid("c6d50a47-f7e6-4482-8be0-4ddfc057fa6e"), "933 Kennedy Hill", new Guid("84312db0-4672-407d-b212-87e550c74428"), new DateTime(2000, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "oduigan1o@plala.or.jp", "Male", "SDBBBB-1A2", "Oliver Duigan", true },
                    { new Guid("cb035f22-e7cf-4907-bd07-91cfee5240f3"), "05043 Katie Parkway", new Guid("7680241d-34b8-4f39-b494-87c27831866c"), new DateTime(1991, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "chutchason1v@theguardian.com", "Male", "SDBBBB-1B1", "Chandler Hutchason", true },
                    { new Guid("d15c6d9f-70b4-48c5-afd3-e71261f1a9be"), "0958 Bashford Park", new Guid("bf9830c7-8e3d-485c-8fb1-9c7f9652fc75"), new DateTime(1992, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "kcowwell1p@mediafire.com", "Female", "SDBBBB-1A3", "Kessiah Cowwell", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryID",
                table: "Persons",
                column: "CountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
