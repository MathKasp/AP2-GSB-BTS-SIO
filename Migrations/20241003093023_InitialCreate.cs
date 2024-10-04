using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace newEmpty.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    AllergieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_Allergie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.AllergieId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Antecedents",
                columns: table => new
                {
                    AntecedentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_Antecedent = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Antecedents", x => x.AntecedentId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medecins",
                columns: table => new
                {
                    MedecinId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_m = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom_m = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date_naissance_m = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Identifiant_m = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotDePasse_m = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medecins", x => x.MedecinId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medicaments",
                columns: table => new
                {
                    MedicamentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_med = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contre_indication = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.MedicamentId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom_p = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Prenom_p = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sexe_p = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Num_secu = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicamentAllergie",
                columns: table => new
                {
                    AllergiesAllergieId = table.Column<int>(type: "int", nullable: false),
                    MedicamentsMedicamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentAllergie", x => new { x.AllergiesAllergieId, x.MedicamentsMedicamentId });
                    table.ForeignKey(
                        name: "FK_MedicamentAllergie_Allergies_AllergiesAllergieId",
                        column: x => x.AllergiesAllergieId,
                        principalTable: "Allergies",
                        principalColumn: "AllergieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicamentAllergie_Medicaments_MedicamentsMedicamentId",
                        column: x => x.MedicamentsMedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "MedicamentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicamentAntecedent",
                columns: table => new
                {
                    AntecedentsAntecedentId = table.Column<int>(type: "int", nullable: false),
                    MedicamentsMedicamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentAntecedent", x => new { x.AntecedentsAntecedentId, x.MedicamentsMedicamentId });
                    table.ForeignKey(
                        name: "FK_MedicamentAntecedent_Antecedents_AntecedentsAntecedentId",
                        column: x => x.AntecedentsAntecedentId,
                        principalTable: "Antecedents",
                        principalColumn: "AntecedentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicamentAntecedent_Medicaments_MedicamentsMedicamentId",
                        column: x => x.MedicamentsMedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "MedicamentId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ordonnances",
                columns: table => new
                {
                    OrdonnanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Posologie = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duree_traitement = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instructions_specifique = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MedecinId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordonnances", x => x.OrdonnanceId);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Medecins_MedecinId",
                        column: x => x.MedecinId,
                        principalTable: "Medecins",
                        principalColumn: "MedecinId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientAllergie",
                columns: table => new
                {
                    AllergiesAllergieId = table.Column<int>(type: "int", nullable: false),
                    PatientsPatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAllergie", x => new { x.AllergiesAllergieId, x.PatientsPatientId });
                    table.ForeignKey(
                        name: "FK_PatientAllergie_Allergies_AllergiesAllergieId",
                        column: x => x.AllergiesAllergieId,
                        principalTable: "Allergies",
                        principalColumn: "AllergieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAllergie_Patients_PatientsPatientId",
                        column: x => x.PatientsPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PatientAntecedent",
                columns: table => new
                {
                    AntecedentsAntecedentId = table.Column<int>(type: "int", nullable: false),
                    PatientsPatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAntecedent", x => new { x.AntecedentsAntecedentId, x.PatientsPatientId });
                    table.ForeignKey(
                        name: "FK_PatientAntecedent_Antecedents_AntecedentsAntecedentId",
                        column: x => x.AntecedentsAntecedentId,
                        principalTable: "Antecedents",
                        principalColumn: "AntecedentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientAntecedent_Patients_PatientsPatientId",
                        column: x => x.PatientsPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MedicamentOrdonnance",
                columns: table => new
                {
                    MedicamentsMedicamentId = table.Column<int>(type: "int", nullable: false),
                    OrdonnancesOrdonnanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentOrdonnance", x => new { x.MedicamentsMedicamentId, x.OrdonnancesOrdonnanceId });
                    table.ForeignKey(
                        name: "FK_MedicamentOrdonnance_Medicaments_MedicamentsMedicamentId",
                        column: x => x.MedicamentsMedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "MedicamentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicamentOrdonnance_Ordonnances_OrdonnancesOrdonnanceId",
                        column: x => x.OrdonnancesOrdonnanceId,
                        principalTable: "Ordonnances",
                        principalColumn: "OrdonnanceId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentAllergie_MedicamentsMedicamentId",
                table: "MedicamentAllergie",
                column: "MedicamentsMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentAntecedent_MedicamentsMedicamentId",
                table: "MedicamentAntecedent",
                column: "MedicamentsMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentOrdonnance_OrdonnancesOrdonnanceId",
                table: "MedicamentOrdonnance",
                column: "OrdonnancesOrdonnanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_MedecinId",
                table: "Ordonnances",
                column: "MedecinId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_PatientId",
                table: "Ordonnances",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergie_PatientsPatientId",
                table: "PatientAllergie",
                column: "PatientsPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientAntecedent_PatientsPatientId",
                table: "PatientAntecedent",
                column: "PatientsPatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicamentAllergie");

            migrationBuilder.DropTable(
                name: "MedicamentAntecedent");

            migrationBuilder.DropTable(
                name: "MedicamentOrdonnance");

            migrationBuilder.DropTable(
                name: "PatientAllergie");

            migrationBuilder.DropTable(
                name: "PatientAntecedent");

            migrationBuilder.DropTable(
                name: "Medicaments");

            migrationBuilder.DropTable(
                name: "Ordonnances");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "Antecedents");

            migrationBuilder.DropTable(
                name: "Medecins");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
