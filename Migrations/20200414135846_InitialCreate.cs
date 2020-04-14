using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MTM.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 15, nullable: false),
                    City = table.Column<string>(maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(maxLength: 15, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Disciple",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(maxLength: 15, nullable: false),
                    FirstName = table.Column<string>(maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 30, nullable: true),
                    LastName = table.Column<string>(maxLength: 20, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    Gender = table.Column<string>(maxLength: 10, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    InitiateDate = table.Column<DateTime>(nullable: false),
                    ClassID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciple", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Disciple_Class_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Class",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscipleID = table.Column<int>(nullable: true),
                    FromTime = table.Column<DateTime>(nullable: false),
                    ToTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registration", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Registration_Disciple_DiscipleID",
                        column: x => x.DiscipleID,
                        principalTable: "Disciple",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disciple_ClassID",
                table: "Disciple",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Registration_DiscipleID",
                table: "Registration",
                column: "DiscipleID");

            var sp = @"CREATE PROCEDURE [dbo].[CountByWeekReport]
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @today DATETIME
                    DECLARE @MondayThisWeek DATETIME
                    DECLARE @SundayThisWeek DATETIME
                    DECLARE @MondayNextWeek DATETIME
                    DECLARE @SundayNextWeek DATETIME
                    DECLARE @MondayPrevWeek DATETIME
                    DECLARE @SundayPrevWeek DATETIME

                    SET @today = GETDATE()
                    SET @MondayThisWeek = CONVERT(date, DATEADD(dd, 0 - (@@DATEFIRST + 5 + DATEPART(dw, @today)) % 7, @today))
                    SET @SundayThisWeek = CONVERT(date, DATEADD(dd, 6 - (@@DATEFIRST + 5 + DATEPART(dw, @today)) % 7, @today))
                    SET @SundayThisWeek = DATEADD(HOUR, 23, @SundayThisWeek)
                    SET @SundayThisWeek = DATEADD(MINUTE, 59, @SundayThisWeek)
                    SET @SundayThisWeek = DATEADD(SECOND, 59, @SundayThisWeek)

                    SET @MondayNextWeek = DATEADD(dd, 7, @MondayThisWeek)
                    SET @SundayNextWeek = DATEADD(dd, 7, @SundayThisWeek)
                    SET @MondayPrevWeek = DATEADD(dd, -7, @MondayThisWeek)
                    SET @SundayPrevWeek = DATEADD(dd, -7, @SundayThisWeek)

                    Select 1 ID, FORMAT(@MondayPrevWeek, 'dd/MM') + ' - ' + FORMAT(@SundayPrevWeek, 'dd/MM') Label, 
                        (select count(*) from [dbo].[Registration]
                        where FromTime between @MondayPrevWeek and @SundayPrevWeek) Y
                    UNION
                    Select 2 ID, FORMAT(@MondayThisWeek, 'dd/MM') + ' - ' + FORMAT(@SundayThisWeek, 'dd/MM') Label, 
                        (select count(*) ThisWeek from [dbo].[Registration]
                        where FromTime between @MondayThisWeek and @SundayThisWeek) Y
                    UNION
                    Select 3 ID, FORMAT(@MondayNextWeek, 'dd/MM') + ' - ' + FORMAT(@SundayNextWeek, 'dd/MM') Label, 
                        (select count(*) from [dbo].[Registration]
                        where FromTime between @MondayNextWeek and @SundayNextWeek) Y
                END";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registration");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Disciple");

            migrationBuilder.DropTable(
                name: "Class");
        }
    }
}
