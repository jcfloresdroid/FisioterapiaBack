using System;
using System.Collections.Generic;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core.Infraestructure.Persistance;

public partial class FisioContext : DbContext
{
    public FisioContext(DbContextOptions<FisioContext> options) : base(options) { }
    
    public virtual DbSet<Cat_Especialidades> Especialidades { get; set; }
    public virtual DbSet<Cat_EstadoCivil> EstadoCivils { get; set; }
    public virtual DbSet<Cat_FlujoVaginal> FlujoVaginals { get; set; }
    public virtual DbSet<Cat_MotivoAlta> MotivoAltas { get; set; }
    public virtual DbSet<Cat_Patologias> Patologias { get; set; }
    public virtual DbSet<Cat_Servicios> Servicios { get; set; }
    public virtual DbSet<Cat_TipoAnticonceptivo> TipoAnticonceptivos { get; set; }
    public virtual DbSet<Cita> Citas { get; set; }
    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }
    public virtual DbSet<Expediente> Expedientes { get; set; }
    public virtual DbSet<ExploracionFisica> ExploracionFisicas { get; set; }
    public virtual DbSet<Fisioterapeuta> Fisioterapeuta { get; set; }
    public virtual DbSet<GinecoObstetrico> GinecoObstetricos { get; set; }
    public virtual DbSet<HeredoFamiliar> HeredoFamiliars { get; set; }
    public virtual DbSet<MapaCorporal> MapaCorporals { get; set; }
    public virtual DbSet<NoPatologico> NoPatologicos { get; set; }
    public virtual DbSet<Paciente> Pacientes { get; set; }
    public virtual DbSet<ProgramaFisioterapeutico> ProgramaFisioterapeuticos { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Revision> Revisions { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cat_Especialidades>(entity =>
        {
            entity.HasKey(e => e.EspecialidadesId);

            entity.ToTable("cat_especialidad");
            
            // Configuración de las propiedades de la entidad Cat_Especialidades
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_EstadoCivil>(entity =>
        {
            entity.HasKey(e => e.EstadoCivilId);

            entity.ToTable("cat_estado_civil");
            
            // Configuración de las propiedades de la entidad Cat_EstadoCivil
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_FlujoVaginal>(entity =>
        {
            entity.HasKey(e => e.FlujoVaginalId);

            entity.ToTable("cat_flujo_vaginal");
            
            // Configuración de las propiedades de la entidad Cat_FlujoVaginal
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_MotivoAlta>(entity =>
        {
            entity.HasKey(e => e.MotivoAltaId);

            entity.ToTable("cat_motivo_alta");
            
            // Configuración de las propiedades de la entidad Cat_MotivoAlta
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_Patologias>(entity =>
        {
            entity.HasKey(e => e.PatologiasId);

            entity.ToTable("cat_patologias");
            
            // Configuración de las propiedades de la entidad Cat_Patologias
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_Servicios>(entity =>
        {
            entity.HasKey(e => e.ServiciosId);

            entity.ToTable("cat_servicios");
            
            // Configuración de las propiedades de la entidad Cat_Servicios
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cat_TipoAnticonceptivo>(entity =>
        {
            entity.HasKey(e => e.TipoAnticonceptivoId);

            entity.ToTable("cat_tipo_anticonceptivo");
            
            // Configuración de las propiedades de la entidad Cat_TipoAnticonceptivo
            entity.HasIndex(e => e.Descripcion).IsUnique();
        });
        
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.ToTable("citas");
            
            entity.HasKey(e => e.CitasId);
            
            // Configuración de las propiedades de la entidad Cita
            entity.Property(e => e.Fecha)
                .HasColumnType("date");
            
            entity.Property(e => e.Hora)
                .HasColumnType("time");

            // Configuración de las relaciones de la entidad Cita
            entity.HasIndex(e => e.PacienteId, "paciente_id");
            entity.HasIndex(e => e.FisioterapeutaId, "fisioterapeuta_id");

            entity.HasOne(d => d.Paciente)
                .WithMany(p => p.Citas)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("citas_ibfk_1");
            
            entity.HasOne(d => d.Fisio)
                .WithMany(p => p.Citas)
                .HasForeignKey(d => d.FisioterapeutaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("citas_ibfk_2");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.ToTable("diagnostico");
            
            entity.HasKey(e => e.DiagnosticoId);

            // Configuración de las propiedades de la entidad Diagnostico
            entity.Property(e => e.FechaAlta)
                .HasColumnType("date");
            
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date");
            
            // Configuración de las relaciones de la entidad Diagnostico
            entity.HasIndex(e => e.ExpedienteId, "expediente_id");
            entity.HasIndex(e => e.ProgramaFisioterapeuticoId, "programafisioterapeutico_id");
            entity.HasIndex(e => e.MapaCorporalId, "mapacorporal_id");
            entity.HasIndex(e => e.MotivoAltaId, "motivoalta_id");
            entity.HasIndex(e => e.MotivoAltaId, "patologias_id");

            entity.HasOne(d => d.Expediente)
                .WithMany(p => p.Diagnosticos)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("diagnostico_ibfk_1");
            
            entity.HasOne(d => d.ProgramaFisioterapeutico)
                .WithOne(p => p.Diagnostico)
                .HasForeignKey<Diagnostico>(d => d.ProgramaFisioterapeuticoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("diagnostico_ibfk_2");
            
            entity.HasOne(d => d.MapaCorporal)
                .WithOne(p => p.Diagnostico)
                .HasForeignKey<Diagnostico>(d => d.MapaCorporalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("diagnostico_ibfk_3");
            
            entity.HasOne(d => d.MotivoAlta)
                .WithMany(p => p.Diagnosticos)
                .HasForeignKey(d => d.MotivoAltaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("diagnostico_ibfk_4");
            
            entity.HasOne(d => d.Patologias)
                .WithMany(p => p.Diagnosticos)
                .HasForeignKey(d => d.PatologiasId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("diagnostico_ibfk_5");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.ToTable("expediente");
            
            entity.HasKey(e => e.ExpedienteId);

            // Configuración de las propiedades de la entidad Expediente
            entity.HasIndex(e => e.Nomenclatura).IsUnique();
            
            // Configuración de las relaciones de la entidad Expediente
            entity.HasIndex(e => e.PacienteId, "paciente_id");
            entity.HasIndex(e => e.HeredoFamiliarId, "heredofamiliar_id");
            entity.HasIndex(e => e.NoPatologicoId, "nopatologico_id");
            
            entity.HasOne(d => d.GinecoObstetrico)
                .WithOne(p => p.Expediente)
                .HasForeignKey<GinecoObstetrico>(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("expediente_ibfk_1");
            
            entity.HasOne(d => d.HeredoFamiliar)
                .WithOne(p => p.Expediente)
                .HasForeignKey<Expediente>(d => d.HeredoFamiliarId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("expediente_ibfk_2");
            
            entity.HasOne(d => d.NoPatologico)
                .WithOne(p => p.Expediente)
                .HasForeignKey<Expediente>(d => d.NoPatologicoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("expediente_ibfk_3");
        });

        modelBuilder.Entity<ExploracionFisica>(entity =>
        {
            entity.ToTable("exploracion_fisica");
            
            entity.HasKey(e => e.ExploracionFisicaId);
        });

        modelBuilder.Entity<Fisioterapeuta>(entity =>
        {
            entity.ToTable("fisioterapeuta");
            
            entity.HasKey(e => e.FisioterapeutaId);
            
            // Configuración de las propiedades de la entidad Fisioterapeuta
            entity.HasIndex(e => e.CedulaProfesional).IsUnique();
            entity.HasIndex(e => e.Telefono).IsUnique();
            entity.HasIndex(e => e.Correo).IsUnique();
            entity.HasIndex(e => e.Nombre).IsUnique();
            
            // Configuración de las relaciones de la entidad Fisioterapeuta
            entity.HasIndex(e => e.EspecialidadId, "especialidad_id");

            entity.HasOne(d => d.Especialidades)
                .WithMany(p => p.Fisioterapeutas)
                .HasForeignKey(d => d.EspecialidadId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fisioterapeuta_ibfk_1");
        });

        modelBuilder.Entity<GinecoObstetrico>(entity =>
        {
            entity.ToTable("gineco_obstetrico");

            entity.HasKey(e => e.GinecoObstetricoId);

            // Configuración de las relaciones de la entidad GinecoObstetrico
            entity.HasIndex(e => e.FlujoVaginalId, "flujo_vaginal_id");
            entity.HasIndex(e => e.TipoAnticonceptivoId, "tipo_anticonceptivo_id");

            entity.HasOne(d => d.CatFlujoVaginal)
                .WithMany(p => p.GinecoObstetricos)
                .HasForeignKey(d => d.FlujoVaginalId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("gineco_obstetrico_ibfk_1");

            entity.HasOne(d => d.CatTipoAnticonceptivo)
                .WithMany(p => p.GinecoObstetricos)
                .HasForeignKey(d => d.TipoAnticonceptivoId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("gineco_obstetrico_ibfk_2");
        });

        modelBuilder.Entity<HeredoFamiliar>(entity =>
        {
            entity.ToTable("heredo_familiar");
            
            entity.HasKey(e => e.HeredoFamiliarId);
        });

        modelBuilder.Entity<MapaCorporal>(entity =>
        {
            entity.ToTable("mapa_corporal");
            
            entity.HasKey(e => e.MapaCorporalId);

            // Configuración de las propiedades de la entidad MapaCorporal
            entity.Property(e => e.Valor)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<int>>(v)
                )
                .HasColumnType("LONGTEXT");
            
            entity.Property(e => e.RangoDolor)
                .HasConversion(
                    x => JsonConvert.SerializeObject(x),
                    x => JsonConvert.DeserializeObject<List<int>>(x)
                )
                .HasColumnType("LONGTEXT");
        });

        modelBuilder.Entity<NoPatologico>(entity =>
        {
            entity.ToTable("no_patologico");
            
            entity.HasKey(e => e.NoPatologicoId);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("paciente");
            
            entity.HasKey(e => e.PacienteId);

            // Configuración de las propiedades de la entidad Paciente
            entity.Property(e => e.Edad)
                .HasColumnType("date");
            entity.HasIndex(e => e.Telefono).IsUnique();
            entity.HasIndex(e => new { e.Nombre, e.Apellido }).IsUnique();
            
            // Configuración de las relaciones de la entidad Paciente
            entity.HasIndex(e => e.EstadoCivilId, "estado_civil_id");
            entity.HasIndex(e => e.FisioterapeutaId, "fisioterapueta_id");
            
            entity.HasOne(d => d.CatEstadoCivil)
                .WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.EstadoCivilId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("paciente_ibfk_1");
            
            entity.HasOne(d => d.Fisioterapeuta)
                .WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.FisioterapeutaId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("paciente_ibfk_2");
            
            // Configuración de la relación uno a uno con Expediente
            entity.HasOne(p => p.Expediente)
                .WithOne(e => e.paciente)
                .HasForeignKey<Expediente>(e => e.PacienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("paciente_ibfk_3");
        });

        modelBuilder.Entity<ProgramaFisioterapeutico>(entity =>
        {
            entity.ToTable("programa_fisioterapeutico");

            entity.HasKey(e => e.ProgramaFisioterapeuticoId);
        });
        
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_token");

            entity.HasKey(e => e.RefreshTokenId);
        });

        modelBuilder.Entity<Revision>(entity =>
        {
            entity.ToTable("revision");

            entity.HasKey(e => e.RevisionId);

            // Configuración de las propiedades de la entidad Revision
            entity.Property(e => e.Fecha)
                .HasColumnType("date");
            
            entity.Property(e => e.Hora)
                .HasColumnType("time");
            
            // Configuración de las relaciones de la entidad Revision
            entity.HasIndex(e => e.DiagnosticoId, "diagnostico_id");
            entity.HasIndex(e => e.ExploracionFisicaId, "exploracionfisica_id");
            entity.HasIndex(e => e.ServicioId, "servicio_id");

            entity.HasOne(d => d.Diagnostico)
                .WithMany(p => p.Revisions)
                .HasForeignKey(d => d.DiagnosticoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("revision_ibfk_1");
            
            entity.HasOne(d => d.ExploracionFisica)
                .WithMany(p => p.Revisions)
                .HasForeignKey(d => d.ExploracionFisicaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("revision_ibfk_2");
            
            entity.HasOne(d => d.Servicio)
                .WithMany(p => p.Revisions)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("revision_ibfk_3");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");

            entity.HasKey(e => e.UsuarioId);

            // Configuración de las propiedades de la entidad Usuario
            entity.HasIndex(e => e.Correo).IsUnique();
            entity.HasIndex(e => e.Telefono).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.EspecialidadId, "especialidad_id");

            entity.HasOne(d => d.Especialidades)
                .WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EspecialidadId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("usuario_ibfk_1");
        });

        base.OnModelCreating(modelBuilder);
    }
}
