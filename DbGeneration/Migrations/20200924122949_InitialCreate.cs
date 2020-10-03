using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DbGeneration.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Action",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(nullable: true),
                    value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Action", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subscriptionUpgrade = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionContext",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    region = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    language = table.Column<string>(nullable: true),
                    currency = table.Column<string>(nullable: true),
                    customerSet = table.Column<string>(nullable: true),
                    segment = table.Column<string>(nullable: true),
                    sourceApplicationName = table.Column<string>(nullable: true),
                    companyNumber = table.Column<int>(nullable: false),
                    businessUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeSelection",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    actionsId = table.Column<int>(nullable: true),
                    instanceId = table.Column<Guid>(nullable: false),
                    path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeSelection", x => x.id);
                    table.ForeignKey(
                        name: "FK_ChangeSelection_Action_actionsId",
                        column: x => x.actionsId,
                        principalTable: "Action",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemItem",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    instanceId = table.Column<Guid>(nullable: false),
                    deltaSelectionsid = table.Column<string>(nullable: true),
                    optionsId = table.Column<int>(nullable: true),
                    path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItemItem_ChangeSelection_deltaSelectionsid",
                        column: x => x.deltaSelectionsid,
                        principalTable: "ChangeSelection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemItem_Options_optionsId",
                        column: x => x.optionsId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseMatrixItem",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    instanceId = table.Column<Guid>(nullable: false),
                    deltaSelectionsid = table.Column<string>(nullable: true),
                    itemsid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMatrixItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_BaseMatrixItem_ChangeSelection_deltaSelectionsid",
                        column: x => x.deltaSelectionsid,
                        principalTable: "ChangeSelection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseMatrixItem_ItemItem_itemsid",
                        column: x => x.itemsid,
                        principalTable: "ItemItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseMatrix",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemsid = table.Column<string>(nullable: true),
                    connections = table.Column<string>(nullable: true),
                    multiplierQuantity = table.Column<int>(nullable: false),
                    isStaging = table.Column<bool>(nullable: false),
                    relationshipContext = table.Column<string>(nullable: true),
                    sellerContextId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseMatrix", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseMatrix_BaseMatrixItem_itemsid",
                        column: x => x.itemsid,
                        principalTable: "BaseMatrixItem",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseMatrix_TransactionContext_sellerContextId",
                        column: x => x.sellerContextId,
                        principalTable: "TransactionContext",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseMatrix_itemsid",
                table: "BaseMatrix",
                column: "itemsid");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMatrix_sellerContextId",
                table: "BaseMatrix",
                column: "sellerContextId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMatrixItem_deltaSelectionsid",
                table: "BaseMatrixItem",
                column: "deltaSelectionsid");

            migrationBuilder.CreateIndex(
                name: "IX_BaseMatrixItem_itemsid",
                table: "BaseMatrixItem",
                column: "itemsid");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeSelection_actionsId",
                table: "ChangeSelection",
                column: "actionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemItem_deltaSelectionsid",
                table: "ItemItem",
                column: "deltaSelectionsid");

            migrationBuilder.CreateIndex(
                name: "IX_ItemItem_optionsId",
                table: "ItemItem",
                column: "optionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseMatrix");

            migrationBuilder.DropTable(
                name: "BaseMatrixItem");

            migrationBuilder.DropTable(
                name: "TransactionContext");

            migrationBuilder.DropTable(
                name: "ItemItem");

            migrationBuilder.DropTable(
                name: "ChangeSelection");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "Action");
        }
    }
}
