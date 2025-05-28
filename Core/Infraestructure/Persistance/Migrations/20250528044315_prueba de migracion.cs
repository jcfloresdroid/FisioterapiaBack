using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Infraestructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class pruebademigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "cat_especialidad",
                columns: new[] { "EspecialidadesId", "Descripcion", "Status" },
                values: new object[] { 1, "Fisioterapeuta", true });

            migrationBuilder.InsertData(
                table: "usuario",
                columns: new[] { "UsuarioId", "Clave", "Correo", "EspecialidadId", "FechaRegistro", "FotoPerfil", "Password", "Telefono", "Username" },
                values: new object[] { 1, "$2a$11$/JWP8Z5kDtqpwAkEG7u5OuG1Iu141q1e/LBsp2y6giQ08bf4lANR2", null, null, new DateTime(2025, 5, 27, 0, 0, 0, 0, DateTimeKind.Local), null, "$2a$11$AwBlNmqamHk4mAaUAR1aeuWyn.xQETPrfskfKV0xkOwwg17tIa5Vi", null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "cat_especialidad",
                keyColumn: "EspecialidadesId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "usuario",
                keyColumn: "UsuarioId",
                keyValue: 1);
        }
    }
}
