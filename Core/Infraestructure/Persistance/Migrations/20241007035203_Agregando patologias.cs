using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Core.Infraestructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Agregandopatologias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_especialidad",
                columns: table => new
                {
                    EspecialidadesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_especialidad", x => x.EspecialidadesId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_estado_civil",
                columns: table => new
                {
                    EstadoCivilId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_estado_civil", x => x.EstadoCivilId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_flujo_vaginal",
                columns: table => new
                {
                    FlujoVaginalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_flujo_vaginal", x => x.FlujoVaginalId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_motivo_alta",
                columns: table => new
                {
                    MotivoAltaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_motivo_alta", x => x.MotivoAltaId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_patologias",
                columns: table => new
                {
                    PatologiasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_patologias", x => x.PatologiasId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_servicios",
                columns: table => new
                {
                    ServiciosId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_servicios", x => x.ServiciosId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cat_tipo_anticonceptivo",
                columns: table => new
                {
                    TipoAnticonceptivoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "varchar(255)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cat_tipo_anticonceptivo", x => x.TipoAnticonceptivoId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exploracion_fisica",
                columns: table => new
                {
                    ExploracionFisicaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fr = table.Column<int>(type: "int", nullable: false),
                    Fc = table.Column<int>(type: "int", nullable: false),
                    Temperatura = table.Column<float>(type: "float", nullable: false),
                    Peso = table.Column<float>(type: "float", nullable: false),
                    Estatura = table.Column<float>(type: "float", nullable: false),
                    Imc = table.Column<float>(type: "float", nullable: false),
                    IndiceCinturaCadera = table.Column<float>(type: "float", nullable: false),
                    SaturacionOxigeno = table.Column<float>(type: "float", nullable: false),
                    PresionArterial = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exploracion_fisica", x => x.ExploracionFisicaId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "heredo_familiar",
                columns: table => new
                {
                    HeredoFamiliarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Padres = table.Column<int>(type: "int", nullable: false),
                    PadresVivos = table.Column<int>(type: "int", nullable: false),
                    PadresCausaMuerte = table.Column<string>(type: "longtext", nullable: true),
                    Hermanos = table.Column<int>(type: "int", nullable: false),
                    HermanosVivos = table.Column<int>(type: "int", nullable: false),
                    HermanosCausaMuerte = table.Column<string>(type: "longtext", nullable: true),
                    Hijos = table.Column<int>(type: "int", nullable: false),
                    HijosVivos = table.Column<int>(type: "int", nullable: false),
                    HijosCausaMuerte = table.Column<string>(type: "longtext", nullable: true),
                    Dm = table.Column<string>(type: "longtext", nullable: false),
                    Hta = table.Column<string>(type: "longtext", nullable: false),
                    Cancer = table.Column<string>(type: "longtext", nullable: false),
                    Alcoholismo = table.Column<string>(type: "longtext", nullable: false),
                    Tabaquismo = table.Column<string>(type: "longtext", nullable: false),
                    Drogas = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_heredo_familiar", x => x.HeredoFamiliarId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mapa_corporal",
                columns: table => new
                {
                    MapaCorporalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<string>(type: "LONGTEXT", nullable: false),
                    RangoDolor = table.Column<string>(type: "LONGTEXT", nullable: false),
                    Nota = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mapa_corporal", x => x.MapaCorporalId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "no_patologico",
                columns: table => new
                {
                    NoPatologicoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MedioLaboral = table.Column<string>(type: "longtext", nullable: false),
                    MedioSociocultural = table.Column<string>(type: "longtext", nullable: false),
                    MedioFisicoambiental = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_no_patologico", x => x.NoPatologicoId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "programa_fisioterapeutico",
                columns: table => new
                {
                    ProgramaFisioterapeuticoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CortoPlazo = table.Column<string>(type: "longtext", nullable: false),
                    MedianoPlazo = table.Column<string>(type: "longtext", nullable: false),
                    LargoPlazo = table.Column<string>(type: "longtext", nullable: false),
                    TratamientoFisioterapeutico = table.Column<string>(type: "longtext", nullable: false),
                    Sugerencias = table.Column<string>(type: "longtext", nullable: false),
                    Pronostico = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programa_fisioterapeutico", x => x.ProgramaFisioterapeuticoId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    RefreshTokenId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    Expiracion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.RefreshTokenId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "fisioterapeuta",
                columns: table => new
                {
                    FisioterapeutaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(255)", nullable: false),
                    Correo = table.Column<string>(type: "varchar(255)", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(255)", nullable: false),
                    CedulaProfesional = table.Column<string>(type: "varchar(255)", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FotoPerfil = table.Column<byte[]>(type: "longblob", nullable: true),
                    EspecialidadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fisioterapeuta", x => x.FisioterapeutaId);
                    table.ForeignKey(
                        name: "fisioterapeuta_ibfk_1",
                        column: x => x.EspecialidadId,
                        principalTable: "cat_especialidad",
                        principalColumn: "EspecialidadesId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(255)", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    Clave = table.Column<string>(type: "longtext", nullable: false),
                    Correo = table.Column<string>(type: "varchar(255)", nullable: true),
                    Telefono = table.Column<string>(type: "varchar(255)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FotoPerfil = table.Column<byte[]>(type: "longblob", nullable: true),
                    EspecialidadId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "usuario_ibfk_1",
                        column: x => x.EspecialidadId,
                        principalTable: "cat_especialidad",
                        principalColumn: "EspecialidadesId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    PacienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(255)", nullable: false),
                    Apellido = table.Column<string>(type: "varchar(255)", nullable: false),
                    Institucion = table.Column<string>(type: "longtext", nullable: false),
                    Domicilio = table.Column<string>(type: "longtext", nullable: false),
                    Ocupacion = table.Column<string>(type: "longtext", nullable: false),
                    Telefono = table.Column<string>(type: "varchar(255)", nullable: false),
                    Notas = table.Column<string>(type: "longtext", nullable: true),
                    Sexo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TipoPaciente = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Edad = table.Column<DateTime>(type: "date", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CodigoPostal = table.Column<int>(type: "int", nullable: false),
                    Foto = table.Column<byte[]>(type: "longblob", nullable: true),
                    EstadoCivilId = table.Column<int>(type: "int", nullable: true),
                    FisioterapeutaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paciente", x => x.PacienteId);
                    table.ForeignKey(
                        name: "paciente_ibfk_1",
                        column: x => x.EstadoCivilId,
                        principalTable: "cat_estado_civil",
                        principalColumn: "EstadoCivilId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "paciente_ibfk_2",
                        column: x => x.FisioterapeutaId,
                        principalTable: "fisioterapeuta",
                        principalColumn: "FisioterapeutaId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "citas",
                columns: table => new
                {
                    CitasId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    Motivo = table.Column<string>(type: "longtext", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    FisioterapeutaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citas", x => x.CitasId);
                    table.ForeignKey(
                        name: "citas_ibfk_1",
                        column: x => x.PacienteId,
                        principalTable: "paciente",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "citas_ibfk_2",
                        column: x => x.FisioterapeutaId,
                        principalTable: "fisioterapeuta",
                        principalColumn: "FisioterapeutaId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "expediente",
                columns: table => new
                {
                    ExpedienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nomenclatura = table.Column<string>(type: "varchar(255)", nullable: false),
                    TipoInterrogatorio = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Responsable = table.Column<string>(type: "longtext", nullable: false),
                    AntecedentesPatologicos = table.Column<string>(type: "longtext", nullable: false),
                    PacienteId = table.Column<int>(type: "int", nullable: false),
                    HeredoFamiliarId = table.Column<int>(type: "int", nullable: false),
                    NoPatologicoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expediente", x => x.ExpedienteId);
                    table.ForeignKey(
                        name: "expediente_ibfk_2",
                        column: x => x.HeredoFamiliarId,
                        principalTable: "heredo_familiar",
                        principalColumn: "HeredoFamiliarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "expediente_ibfk_3",
                        column: x => x.NoPatologicoId,
                        principalTable: "no_patologico",
                        principalColumn: "NoPatologicoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "paciente_ibfk_3",
                        column: x => x.PacienteId,
                        principalTable: "paciente",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "diagnostico",
                columns: table => new
                {
                    DiagnosticoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false),
                    Refiere = table.Column<string>(type: "longtext", nullable: false),
                    Categoria = table.Column<string>(type: "longtext", nullable: false),
                    DiagnosticoPrevio = table.Column<string>(type: "longtext", nullable: false),
                    TerapeuticaEmpleada = table.Column<string>(type: "longtext", nullable: false),
                    DiagnosticoFuncional = table.Column<string>(type: "longtext", nullable: false),
                    PadecimientoActual = table.Column<string>(type: "longtext", nullable: false),
                    Inspeccion = table.Column<string>(type: "longtext", nullable: false),
                    ExploracionFisicaCuadro = table.Column<string>(type: "longtext", nullable: false),
                    EstudiosComplementarios = table.Column<string>(type: "longtext", nullable: false),
                    DiagnosticoNosologico = table.Column<string>(type: "longtext", nullable: false),
                    DiagnosticoInicial = table.Column<string>(type: "longtext", nullable: true),
                    DiagnosticoFinal = table.Column<string>(type: "longtext", nullable: true),
                    FrecuenciaTratamiento = table.Column<string>(type: "longtext", nullable: true),
                    Estatus = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "date", nullable: true),
                    FechaInicio = table.Column<DateTime>(type: "date", nullable: false),
                    MotivoAltaId = table.Column<int>(type: "int", nullable: true),
                    PatologiasId = table.Column<int>(type: "int", nullable: true),
                    ProgramaFisioterapeuticoId = table.Column<int>(type: "int", nullable: false),
                    MapaCorporalId = table.Column<int>(type: "int", nullable: false),
                    ExpedienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diagnostico", x => x.DiagnosticoId);
                    table.ForeignKey(
                        name: "diagnostico_ibfk_1",
                        column: x => x.ExpedienteId,
                        principalTable: "expediente",
                        principalColumn: "ExpedienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnostico_ibfk_2",
                        column: x => x.ProgramaFisioterapeuticoId,
                        principalTable: "programa_fisioterapeutico",
                        principalColumn: "ProgramaFisioterapeuticoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnostico_ibfk_3",
                        column: x => x.MapaCorporalId,
                        principalTable: "mapa_corporal",
                        principalColumn: "MapaCorporalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "diagnostico_ibfk_4",
                        column: x => x.MotivoAltaId,
                        principalTable: "cat_motivo_alta",
                        principalColumn: "MotivoAltaId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "diagnostico_ibfk_5",
                        column: x => x.PatologiasId,
                        principalTable: "cat_patologias",
                        principalColumn: "PatologiasId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gineco_obstetrico",
                columns: table => new
                {
                    GinecoObstetricoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Fum = table.Column<string>(type: "longtext", nullable: false),
                    Fpp = table.Column<string>(type: "longtext", nullable: false),
                    Menarca = table.Column<string>(type: "longtext", nullable: false),
                    Ritmo = table.Column<string>(type: "longtext", nullable: false),
                    Cirugias = table.Column<string>(type: "longtext", nullable: false),
                    EdadGestional = table.Column<int>(type: "int", nullable: false),
                    Semanas = table.Column<int>(type: "int", nullable: false),
                    Gestas = table.Column<int>(type: "int", nullable: false),
                    Partos = table.Column<int>(type: "int", nullable: false),
                    Cesareas = table.Column<int>(type: "int", nullable: false),
                    Abortos = table.Column<int>(type: "int", nullable: false),
                    FlujoVaginalId = table.Column<int>(type: "int", nullable: true),
                    TipoAnticonceptivoId = table.Column<int>(type: "int", nullable: true),
                    ExpedienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gineco_obstetrico", x => x.GinecoObstetricoId);
                    table.ForeignKey(
                        name: "expediente_ibfk_1",
                        column: x => x.ExpedienteId,
                        principalTable: "expediente",
                        principalColumn: "ExpedienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "gineco_obstetrico_ibfk_1",
                        column: x => x.FlujoVaginalId,
                        principalTable: "cat_flujo_vaginal",
                        principalColumn: "FlujoVaginalId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "gineco_obstetrico_ibfk_2",
                        column: x => x.TipoAnticonceptivoId,
                        principalTable: "cat_tipo_anticonceptivo",
                        principalColumn: "TipoAnticonceptivoId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "revision",
                columns: table => new
                {
                    RevisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Notas = table.Column<string>(type: "longtext", nullable: false),
                    FolioPago = table.Column<string>(type: "longtext", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    ExploracionFisicaId = table.Column<int>(type: "int", nullable: false),
                    DiagnosticoId = table.Column<int>(type: "int", nullable: false),
                    ServicioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revision", x => x.RevisionId);
                    table.ForeignKey(
                        name: "revision_ibfk_1",
                        column: x => x.DiagnosticoId,
                        principalTable: "diagnostico",
                        principalColumn: "DiagnosticoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "revision_ibfk_2",
                        column: x => x.ExploracionFisicaId,
                        principalTable: "exploracion_fisica",
                        principalColumn: "ExploracionFisicaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "revision_ibfk_3",
                        column: x => x.ServicioId,
                        principalTable: "cat_servicios",
                        principalColumn: "ServiciosId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_cat_especialidad_Descripcion",
                table: "cat_especialidad",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_estado_civil_Descripcion",
                table: "cat_estado_civil",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_flujo_vaginal_Descripcion",
                table: "cat_flujo_vaginal",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_motivo_alta_Descripcion",
                table: "cat_motivo_alta",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_patologias_Descripcion",
                table: "cat_patologias",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_servicios_Descripcion",
                table: "cat_servicios",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cat_tipo_anticonceptivo_Descripcion",
                table: "cat_tipo_anticonceptivo",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "fisioterapeuta_id",
                table: "citas",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "paciente_id",
                table: "citas",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_diagnostico_MapaCorporalId",
                table: "diagnostico",
                column: "MapaCorporalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_diagnostico_PatologiasId",
                table: "diagnostico",
                column: "PatologiasId");

            migrationBuilder.CreateIndex(
                name: "IX_diagnostico_ProgramaFisioterapeuticoId",
                table: "diagnostico",
                column: "ProgramaFisioterapeuticoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "expediente_id",
                table: "diagnostico",
                column: "ExpedienteId");

            migrationBuilder.CreateIndex(
                name: "mapacorporal_id",
                table: "diagnostico",
                column: "MapaCorporalId");

            migrationBuilder.CreateIndex(
                name: "motivoalta_id",
                table: "diagnostico",
                column: "MotivoAltaId");

            migrationBuilder.CreateIndex(
                name: "patologias_id",
                table: "diagnostico",
                column: "MotivoAltaId");

            migrationBuilder.CreateIndex(
                name: "programafisioterapeutico_id",
                table: "diagnostico",
                column: "ProgramaFisioterapeuticoId");

            migrationBuilder.CreateIndex(
                name: "IX_expediente_HeredoFamiliarId",
                table: "expediente",
                column: "HeredoFamiliarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expediente_NoPatologicoId",
                table: "expediente",
                column: "NoPatologicoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expediente_Nomenclatura",
                table: "expediente",
                column: "Nomenclatura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_expediente_PacienteId",
                table: "expediente",
                column: "PacienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "heredofamiliar_id",
                table: "expediente",
                column: "HeredoFamiliarId");

            migrationBuilder.CreateIndex(
                name: "nopatologico_id",
                table: "expediente",
                column: "NoPatologicoId");

            migrationBuilder.CreateIndex(
                name: "paciente_id1",
                table: "expediente",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_fisioterapeuta_CedulaProfesional",
                table: "fisioterapeuta",
                column: "CedulaProfesional",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fisioterapeuta_Correo",
                table: "fisioterapeuta",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fisioterapeuta_Nombre",
                table: "fisioterapeuta",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fisioterapeuta_Telefono",
                table: "fisioterapeuta",
                column: "Telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "especialidad_id",
                table: "fisioterapeuta",
                column: "EspecialidadId");

            migrationBuilder.CreateIndex(
                name: "IX_gineco_obstetrico_ExpedienteId",
                table: "gineco_obstetrico",
                column: "ExpedienteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "flujo_vaginal_id",
                table: "gineco_obstetrico",
                column: "FlujoVaginalId");

            migrationBuilder.CreateIndex(
                name: "tipo_anticonceptivo_id",
                table: "gineco_obstetrico",
                column: "TipoAnticonceptivoId");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_Nombre_Apellido",
                table: "paciente",
                columns: new[] { "Nombre", "Apellido" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paciente_Telefono",
                table: "paciente",
                column: "Telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "estado_civil_id",
                table: "paciente",
                column: "EstadoCivilId");

            migrationBuilder.CreateIndex(
                name: "fisioterapueta_id",
                table: "paciente",
                column: "FisioterapeutaId");

            migrationBuilder.CreateIndex(
                name: "diagnostico_id",
                table: "revision",
                column: "DiagnosticoId");

            migrationBuilder.CreateIndex(
                name: "exploracionfisica_id",
                table: "revision",
                column: "ExploracionFisicaId");

            migrationBuilder.CreateIndex(
                name: "servicio_id",
                table: "revision",
                column: "ServicioId");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Correo",
                table: "usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Telefono",
                table: "usuario",
                column: "Telefono",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Username",
                table: "usuario",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "especialidad_id1",
                table: "usuario",
                column: "EspecialidadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "citas");

            migrationBuilder.DropTable(
                name: "gineco_obstetrico");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "revision");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "cat_flujo_vaginal");

            migrationBuilder.DropTable(
                name: "cat_tipo_anticonceptivo");

            migrationBuilder.DropTable(
                name: "diagnostico");

            migrationBuilder.DropTable(
                name: "exploracion_fisica");

            migrationBuilder.DropTable(
                name: "cat_servicios");

            migrationBuilder.DropTable(
                name: "expediente");

            migrationBuilder.DropTable(
                name: "programa_fisioterapeutico");

            migrationBuilder.DropTable(
                name: "mapa_corporal");

            migrationBuilder.DropTable(
                name: "cat_motivo_alta");

            migrationBuilder.DropTable(
                name: "cat_patologias");

            migrationBuilder.DropTable(
                name: "heredo_familiar");

            migrationBuilder.DropTable(
                name: "no_patologico");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "cat_estado_civil");

            migrationBuilder.DropTable(
                name: "fisioterapeuta");

            migrationBuilder.DropTable(
                name: "cat_especialidad");
        }
    }
}
