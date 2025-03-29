using Core.Domain.Entities;
using Core.Domain.Helpers;
using Core.Domain.Resource;
using Core.Infraestructure.Persistance;
using Core.Services.Interfaz;
using MediatR;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Paragraph = iTextSharp.text.Paragraph;

namespace Core.Features.Reportes;

public record ReporteExpediente : IRequest<ReporteExpedienteResponse>
{
    public string DiagnosticoId { get; set; }
}

public class ReporteExpedienteHandler : IRequestHandler<ReporteExpediente, ReporteExpedienteResponse>
{
    private readonly IUtilPdf _utilPdf;
    private readonly FisioContext _context;

    public ReporteExpedienteHandler(IUtilPdf utilPdf, FisioContext context)
    {
        _utilPdf = utilPdf;
        _context = context;
    }

    public async Task<ReporteExpedienteResponse> Handle(ReporteExpediente request, CancellationToken cancellationToken)
    {
        var diagnostico = await _context.Diagnosticos
            .Include(x => x.ProgramaFisioterapeutico)
            .Include(x => x.MapaCorporal)
            .Include(x => x.MotivoAlta)
            .Include(x => x.Patologias)
            .FirstOrDefaultAsync(x => x.DiagnosticoId == request.DiagnosticoId.HashIdInt());
        
        var expediente = await _context.Expedientes
            .Include(x => x.HeredoFamiliar)
            .Include(x => x.GinecoObstetrico)
                .ThenInclude(x => x.CatFlujoVaginal)
            .Include(x => x.NoPatologico)
            .FirstOrDefaultAsync(x => x.ExpedienteId == diagnostico.ExpedienteId);
        
        var paciente = await _context.Pacientes
            .Include(x => x.Fisioterapeuta)
            .Include(x => x.CatEstadoCivil)
            .FirstOrDefaultAsync(x => x.PacienteId == expediente.PacienteId);
        
        var revision = await _context.Revisions
            .AsNoTracking()
            .Include(x => x.ExploracionFisica)
            .Where(x => x.Diagnostico.Expediente.paciente.PacienteId == paciente.PacienteId)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.RevisionId)
            .FirstOrDefaultAsync();
        
        var revisiones = await _context.Revisions
            .AsNoTracking()
            .Include(x => x.ExploracionFisica)
            .Include(x => x.Servicio)
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Hora)
            .ThenByDescending(x => x.RevisionId)
            .Where(x => x.DiagnosticoId == request.DiagnosticoId.HashIdInt())
            .ToListAsync();
        
        using (var pdf = await GetReport(diagnostico, expediente, paciente, revision, revisiones))
        {
            var response = Convert.ToBase64String(pdf.ToArray());
            return new ReporteExpedienteResponse
            {
                Reporte = response
            };
        }
    }

    public async Task<MemoryStream> GetReport(Domain.Entities.Diagnostico diag, Expediente exp, Paciente pac, Revision rev, List<Revision> revs)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            Document doc = new Document();
            doc.SetMargins(20, 20, 100, 0);
            PdfWriter pw = PdfWriter.GetInstance(doc, stream);
            pw.CloseStream = false;

            var pe = new CrearEncabezado();
            pw.PageEvent = pe;

            doc.Open();

            // Encabezado
            Encabezado(doc);
            doc.Add(HojaFrontal(doc, exp, pac));
            doc.Add(HojaFrontalP2(diag, pac.Fisioterapeuta));
            doc.NewPage();
            doc.Add(HistoriaClinica(doc, diag, exp, pac));
            doc.NewPage();
            doc.Add(Antecedentes(doc, exp.HeredoFamiliar));
            doc.NewPage();
            doc.Add(NoPatologico(doc, exp.NoPatologico));
            doc.Add(Patologico(doc, exp));
            if(!pac.Sexo)
                doc.Add(Gineco(doc, exp.GinecoObstetrico));
            doc.NewPage();
            doc.Add(Padecimiento(doc, diag));
            doc.Add(Mapa(doc, diag.MapaCorporal, pac.Sexo));
            if(pac.Sexo)
                Imagenes(pw, diag.MapaCorporal);
            else
                ImagenesMujer(pw, diag.MapaCorporal);
            doc.NewPage();
            doc.Add(Terapeutica(doc, diag));
            doc.Add(DPrevio(doc, diag));
            doc.Add(exploracion(doc, rev.ExploracionFisica));
            doc.Add(inspeccion(doc, diag));
            doc.NewPage();
            doc.Add(ExploracionFisicaCuadro(doc, diag));
            doc.Add(pruebas(doc, diag));
            doc.Add(nosologico(doc, diag));
            doc.NewPage();
            doc.Add(programa(doc, diag.ProgramaFisioterapeutico));
            doc.Add(tratamiento(doc, diag.ProgramaFisioterapeutico));
            doc.Add(sugerencias(doc, diag.ProgramaFisioterapeutico));
            doc.Add(pronosticos(doc, diag.ProgramaFisioterapeutico));
            doc.Add(fisio(doc, pac.Fisioterapeuta));
            doc.NewPage();
            Encabezado2(doc);
            doc.Add(hojaAlta(doc, exp, diag, pac));
            doc.NewPage();
            revisiones(doc, revs);
            doc.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;
            return stream;
        }
    }
    
    public PdfPTable hojaAlta(Document doc, Expediente exp, Domain.Entities.Diagnostico diag, Paciente pac)
    {
        PdfPTable datosGenerales = new PdfPTable(3);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 10;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        List<bool> superior = new List<bool> { true, true, false, true };
        List<bool> inferior = new List<bool> { false, true, true, true };
        List<bool> without = new List<bool> { false, false, false, false };
        
        //Primera fila
        _utilPdf.CellEdit(datosGenerales, " ", baseFont, 10, 5, 1, true, true, 5f, 0, 0, 0, 0, true);
        _utilPdf.CellEdits(datosGenerales, "Número de folio:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Nombre del usuario:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Fecha de inicio:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, exp.Nomenclatura, fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, $"{pac.Nombre} {pac.Apellido}", fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, diag.FechaInicio.ToString("dd/MM/yyyy"), fontResponse, 11, 1, 90, false, false, inferior);
        //Segunda fila
        _utilPdf.CellEdits(datosGenerales, "Diagnóstico inicial:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Fisioterapeuta:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Frecuencia de tratamiento:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, diag.DiagnosticoInicial, fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, pac.Fisioterapeuta.Nombre, fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, diag.FrecuenciaTratamiento, fontResponse, 11, 1, 90, false, false, inferior);
        //Tercera fila
        _utilPdf.CellEdits(datosGenerales, "Diagnóstico final:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Categoría:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, "Fecha de alta de servicio:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, diag.DiagnosticoFinal, fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, diag.Categoria, fontResponse, 11, 1, 90, false, false, inferior);
        _utilPdf.CellEdits(datosGenerales, diag.FechaAlta?.ToString("dd/MM/yyyy"), fontResponse, 11, 1, 90, false, false, inferior);
        
        _utilPdf.CellEdit(datosGenerales, " ", baseFont, 10, 5, 1, true, true, 15f, 0, 0, 0, 0, true);
        _utilPdf.CellEdits(datosGenerales, "Motivo del alta del servicio:", baseFont, 14, 3, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, diag.MotivoAlta.Descripcion, fontResponse, 11, 3, 70, false, true, inferior);
        
        _utilPdf.CellEdits(datosGenerales, " ", baseFont, 14, 2, 20, true, true, without);
        _utilPdf.CellEdits(datosGenerales, "Firma del fisioterapeuta:", baseFont, 14, 1, 20, true, true, superior);
        _utilPdf.CellEdits(datosGenerales, " ", fontResponse, 14, 2, 80, false, false, without);
        _utilPdf.CellEdits(datosGenerales, " ", fontResponse, 14, 1, 80, false, false, inferior);
        
        return datosGenerales;
    }
    
    public void AgregarImagenesEnTabla(PdfPTable tableLayout, string[] rutasImagenes)
    {
        foreach (string rutaImagen in rutasImagenes)
        {
            Image img = Image.GetInstance(rutaImagen);
            PdfPCell cell = new PdfPCell(img)
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                Border = PdfPCell.NO_BORDER,
                BackgroundColor = new BaseColor(235, 245, 255),
                PaddingTop = 10f,
            };
            tableLayout.AddCell(cell);
        }
    }
    
    public void revisiones(Document doc, List<Revision> revs)
    {
        string rutaBase = Directory.GetCurrentDirectory();
        string estatura = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "Estatura.png");
        string cadiaca = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "FreCardiaca.png");
        string respiratoria = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "FreRespiratoria.png");
        string icc = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "ICC.png");
        string imc = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "IMC.png");
        string oxigeno = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "Oxigeno.png");
        string peso = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "Peso.png");
        string presion = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "Presion.png");
        string temperatura = Path.Combine(rutaBase, "wwwroot", "ExploracionFisica", "Temperatura.png");

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("NOTAS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);

        var emptyCell = new PdfPCell(new Phrase(" "))
        {
            Border = PdfPCell.NO_BORDER,
            Colspan = 9
        };
        
        string[] rutasImagenes = { estatura, peso, temperatura, cadiaca, respiratoria, presion, oxigeno, icc, imc };
        
        int contador = 0;
        
        PdfPTable datosGenerales = new PdfPTable(9);
        
        foreach (var rev in revs)
        {
            if (contador == 0)
            {
                datosGenerales = new PdfPTable(9);

                datosGenerales.WidthPercentage = 100;
                datosGenerales.HeaderRows = 1;
                datosGenerales.SpacingBefore = 20;
                datosGenerales.SpacingAfter = 10;
            }
            
            _utilPdf.Revisiones(datosGenerales, rev.Fecha.ToString("yyyy-MM-dd") + " " + rev.Hora, baseFont, 12, 9);
            _utilPdf.Revisiones(datosGenerales, "Servicio: " + rev.Servicio.Descripcion, baseFont, 12, 9);
            _utilPdf.Revisiones(datosGenerales, rev.Notas, baseFont, 13, 9);
            datosGenerales.AddCell(emptyCell);
            
            AgregarImagenesEnTabla(datosGenerales, rutasImagenes);
            
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Estatura.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Peso.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Temperatura.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Fc.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Fr.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.PresionArterial, baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.SaturacionOxigeno.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.IndiceCinturaCadera.ToString(), baseFont, 11, 1, false);
            _utilPdf.ExploracionF(datosGenerales, rev.ExploracionFisica.Imc.ToString(), baseFont, 11, 1, false);
            
            _utilPdf.ExploracionF(datosGenerales, "m", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "kg", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "c\u00b0", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "bpm", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "rpm", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "mmHg", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "%", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "icc", baseFont, 11, 1, true);
            _utilPdf.ExploracionF(datosGenerales, "imc", baseFont, 11, 1, true);
            datosGenerales.AddCell(emptyCell);
            
            contador++;

            if (contador == 3)
            {
                contador = 0;
                doc.Add(datosGenerales);
                doc.NewPage();
            }
        }
        
        if(contador != 0)
            doc.Add(datosGenerales);
        
        //return datosGenerales;
    }
    
    public PdfPTable fisio(Document doc, Fisioterapeuta fisio)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 10;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("FISIOTERAPEUTA\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, "Nombre y Cedula profesional", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, $"{fisio.Nombre} {fisio.CedulaProfesional}", fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        
        return datosGenerales;
    }
    
    public PdfPTable pronosticos(Document doc, ProgramaFisioterapeutico prog)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 10;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("PRONÓSTICO\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, prog.Pronostico, fontResponse, 10, 1, 1, true, true, 60f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable sugerencias(Document doc, ProgramaFisioterapeutico prog)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 10;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("SUGERENCIAS Y RECOMENDACIONES\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, prog.Sugerencias, fontResponse, 10, 1, 1, true, true, 80f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable tratamiento(Document doc, ProgramaFisioterapeutico prog)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 10;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("TRATAMIENTO FISIOTERAPEUTICO\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, prog.TratamientoFisioterapeutico, fontResponse, 10, 1, 1, true, true, 100f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable programa(Document doc, ProgramaFisioterapeutico prog)
    {
        PdfPTable datosGenerales = new PdfPTable(5);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("PROGRAMA FISIOTERAPÉUTA\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, " ", baseFont, 10, 5, 1, true, true, 5f, 0, 0, 0, 0, true);
        _utilPdf.AddCellWithBorder(datosGenerales, "OBJETIVOS", baseFont, 14, 1, 6, true);
        _utilPdf.CellEdit(datosGenerales, "A corto plazo:", baseFont, 14, 4, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, prog.CortoPlazo, fontResponse, 10, 4, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, "A mediano plazo:", baseFont, 14, 4, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, prog.MedianoPlazo, fontResponse, 10, 4, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, "A largo plazo:", baseFont, 14, 4, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, prog.LargoPlazo, fontResponse, 10, 4, 1, true, true, 20f, 0, 1, 1, 1, false);
        
        return datosGenerales;
    }
    
    public PdfPTable nosologico(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("DIAGNOSTICO NOSOLÓGICO Y/O FUNCIONAL\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, $"{diag.DiagnosticoNosologico} {diag.DiagnosticoFuncional}", fontResponse, 10, 1, 1, true, true, 60f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable pruebas(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 15;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
            Paragraph encabezado = new Paragraph("PRUEBAS ESPECIALES Y ESTUDIOS COMPLEMENTARIOS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, diag.EstudiosComplementarios, fontResponse, 10, 1, 1, true, true, 120f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    
    public PdfPTable ExploracionFisicaCuadro(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 15;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("Exploracion Física\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, diag.ExploracionFisicaCuadro, fontResponse, 10, 1, 1, true, true, 120f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable inspeccion(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("INSPECCION GENERAL Y ESPECÍFICA\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, diag.Inspeccion, fontResponse, 10, 1, 1, true, true, 120f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable exploracion(Document doc, ExploracionFisica ef)
    {
        PdfPTable datosGenerales = new PdfPTable(3);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("EXPLORACIÓN FÍSICA\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.CellEdit(datosGenerales, "Temperatura:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "F.R.:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "F.C.:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, ef.Temperatura.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.Fr.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.Fc.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        
        _utilPdf.CellEdit(datosGenerales, "presión arterial:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Peso:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Estatura:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, ef.PresionArterial, fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.Peso.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.Estatura.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        
        _utilPdf.CellEdit(datosGenerales, "Índice de masa corporal:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Índice cintura-cadera:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Saturación de Oxígeno:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, ef.Imc.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.IndiceCinturaCadera.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, ef.SaturacionOxigeno.ToString(), fontResponse, 10, 1, 1, true, true, 20f, 0, 1, 1, 1, false);

        return datosGenerales;
    }
    
    public PdfPTable DPrevio(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("DIAGNÓSTICOS PREVIOS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.AddCellWithHeigth(datosGenerales, diag.DiagnosticoPrevio, fontResponse, 10, 1, true, true, 80f);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });

        return datosGenerales;
    }
    
    public PdfPTable Terapeutica(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("TERAPEUTICA EMPLEADA Y TRATAMIENTOS A FINES\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);

        _utilPdf.AddCellWithHeigth(datosGenerales, diag.TerapeuticaEmpleada, fontResponse, 10, 1, true, true, 100f);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public void ImagenesMujer(PdfWriter writer, MapaCorporal map)
    {
        PdfContentByte cb = writer.DirectContent;

        string rutaBase = Directory.GetCurrentDirectory();
        string rutaImagen = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "GeneralMujer.png");

        // Diccionario de imagenes
        var imageDictionary = ImagenesMapaM.MapaMujer();
        
        // puntos seleccionados
        int[] selectedImages = map.Valor.ToArray();
        
        //Agregamos por defecto la imagen global
        Image imgGlobal = Image.GetInstance(rutaImagen);
        imgGlobal.SetAbsolutePosition(87, 280);
        cb.AddImage(imgGlobal);
        
        //Agregamos todos los puntos seleccionados
        foreach (int key in selectedImages)
        {
            if (imageDictionary.TryGetValue(key, out string imageUrl))
            {
                Image img = Image.GetInstance(imageUrl);
                img.SetAbsolutePosition(87, 280);
                cb.AddImage(img);
            }
        }
    }
    
    public void Imagenes(PdfWriter writer, MapaCorporal map)
    {
        PdfContentByte cb = writer.DirectContent;

        string rutaBase = Directory.GetCurrentDirectory();
        string rutaImagen = Path.Combine(rutaBase, "wwwroot", "MapaCorporal", "Global.png");

        // Diccionario de imagenes
        var imageDictionary = ImagenesMapa.MapaHombre();
        
        // puntos seleccionados
        int[] selectedImages = map.Valor.ToArray();
        
        //Agregamos por defecto la imagen global
        Image imgGlobal = Image.GetInstance(rutaImagen);
        imgGlobal.SetAbsolutePosition(87, 280);
        cb.AddImage(imgGlobal);
        
        //Agregamos todos los puntos seleccionados
        foreach (int key in selectedImages)
        {
            if (imageDictionary.TryGetValue(key, out string imageUrl))
            {
                Image img = Image.GetInstance(imageUrl);
                img.SetAbsolutePosition(87, 280);
                cb.AddImage(img);
            }
        }
    }
    
    public PdfPTable Mapa(Document doc, MapaCorporal map, bool sex)
    {
        PdfPTable datosGenerales = new PdfPTable(4);

        float[] headers = { 15, 35, 15, 35 }; 
        datosGenerales.SetWidths(headers);
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("PRESENTACION DE DOLOR EN MAPA CORPORAL\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14))
        {
            SpacingAfter = 220f
        };
        doc.Add(encabezado);

        Dictionary<int, string> diccionario = new Dictionary<int, string>();
        
        if(sex)
            diccionario = PuntosMapa.MapaHombre();
        else
            diccionario = PuntosMapa.MapaMujer();
        
        for (int i = 0; i < map.Valor.Count; i++)
        {
            _utilPdf.CellEdit(datosGenerales, "Punto: ", baseFont, 14, 1, 1, true, true, 20f, 1, 0, 0, 1, true);
            _utilPdf.CellEdit(datosGenerales, diccionario[map.Valor[i]], fontResponse, 10, 1, 1, true, true, 20f, 1, 0, 0, 0, true);
            _utilPdf.CellEdit(datosGenerales, "Rango dolor:", baseFont, 14, 1, 1, true, true, 20f, 1, 0, 0, 1, true);
            _utilPdf.CellEdit(datosGenerales, map.RangoDolor[i].ToString(), fontResponse, 10, 1, 1, true, true, 20f, 1, 1, 0, 0, true);
        }
        
        _utilPdf.CellEdit(datosGenerales, "Nota:", baseFont, 14, 4, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, map.Nota, fontResponse, 10, 4, 1, true, true, 40f, 0, 1, 1, 1, false);
   
        return datosGenerales;
    }

    public PdfPTable Padecimiento(Document doc, Domain.Entities.Diagnostico diag)
    {
        PdfPTable datosGenerales = new PdfPTable(1);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("PADECIMIENTO ACTUAL\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);

        _utilPdf.CellEdit(datosGenerales, diag.PadecimientoActual, fontResponse, 10, 1, 1, true, true, 140f, 1, 1, 1, 1, true);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable Gineco(Document doc, GinecoObstetrico gc)
    {
        PdfPTable datosGenerales = new PdfPTable(4);

        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("ANTECEDENTES GINECO-OBSTÉTRICOS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);

        _utilPdf.CellEdit(datosGenerales, "FUM:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "FPP:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 0, true);
        _utilPdf.CellEdit(datosGenerales, "Edad gestional:", baseFont, 14, 1, 1, true, true, 20f, 1, 0, 0, 0, true);
        _utilPdf.CellEdit(datosGenerales, "Semanas:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 0, true);
        _utilPdf.CellEdit(datosGenerales, gc.Fum, fontResponse, 10, 1, 1, false, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, gc.Fpp, fontResponse, 10, 1, 1, false, true, 20f, 0, 1, 1, 0, false);
        _utilPdf.CellEdit(datosGenerales, gc.EdadGestional.ToString(), fontResponse, 10, 1, 1, false, true, 20f, 0, 0, 1, 0, false);
        _utilPdf.CellEdit(datosGenerales, gc.Semanas.ToString(), fontResponse, 10, 1, 1, false, true, 20f, 0, 1, 1, 0, false);
    
        _utilPdf.CellEdit(datosGenerales, "Edad de la menarca:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Ritmo Menstrual:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Flujo vaginal:", baseFont, 14, 2, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, gc.Menarca, fontResponse, 10, 1, 1, false, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, gc.Ritmo, fontResponse, 10, 1, 1, false, true, 20f, 0, 1, 1, 1, false);
        _utilPdf.CellEdit(datosGenerales, gc.CatFlujoVaginal.Descripcion, fontResponse, 10, 2, 1, false, true, 20f, 0, 1, 1, 1, false);
        
        _utilPdf.CellEdit(datosGenerales, "Gestas:", baseFont, 14, 1, 1, true, true, 20f, 1, 1, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Partos:", baseFont, 14, 1, 1, true, true, 20f, 1, 0, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, gc.Gestas.ToString(), fontResponse, 10, 2, 1, true, true, 20f, 1, 1, 0, 0, false);
        _utilPdf.CellEdit(datosGenerales, gc.Partos.ToString(), fontResponse, 10, 1, 2, false, false, 20f, 0, 1, 1, 1, true);
        _utilPdf.CellEdit(datosGenerales, "Cesareas:", baseFont, 14, 1, 1, true, true, 20f, 0, 0, 0, 1, true);
        _utilPdf.CellEdit(datosGenerales, gc.Cesareas.ToString(), fontResponse, 10, 2, 1, true, true, 20f, 0, 1, 0, 0, false);
        _utilPdf.CellEdit(datosGenerales, "Abortos:", baseFont, 14, 1, 1, true, true, 20f, 0, 0, 1, 1, true);
        _utilPdf.CellEdit(datosGenerales, gc.Abortos.ToString(), fontResponse, 10, 2, 1, true, true, 20f, 0, 1, 1, 0, false);
        
        return datosGenerales;
    }
    
    public PdfPTable Patologico(Document doc, Expediente exp) {
        PdfPTable datosGenerales = new PdfPTable(1);
        
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("ANTECEDENTES PERSONALES PATOLÓGICOS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.AddCellWithHeigth(datosGenerales, exp.AntecedentesPatologicos, fontResponse, 10, 1, true, true, 100f);
        datosGenerales.AddCell(new PdfPCell(new Phrase(" ")) { Border = PdfPCell.NO_BORDER });
        
        return datosGenerales;
    }
    
    public PdfPTable NoPatologico(Document doc, NoPatologico np) {
        PdfPTable datosGenerales = new PdfPTable(1);
        
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("ANTECEDENTES PERSONALES NO PATOLÓGICOS\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(encabezado);
        
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Medio laboral", baseFont, 14, 1, true);
        _utilPdf.AddCellWithHeigthTop(datosGenerales, np.MedioLaboral, fontResponse, 10, 1, true, true, 80f);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Medio socio-cultural", baseFont, 14, 1, true);
        _utilPdf.AddCellWithHeigthTop(datosGenerales, np.MedioSociocultural, fontResponse, 10, 1, true, true, 80f);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Medio fisico-ambiental", baseFont, 14, 1, true);
        _utilPdf.AddCellWithHeigthTop(datosGenerales, np.MedioFisicoambiental, fontResponse, 10, 1, true, true, 80f);
        
        return datosGenerales;
    }

    public PdfPTable Antecedentes(Document doc, HeredoFamiliar hf) {
        PdfPTable datosGenerales = new PdfPTable(4);
        
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("ANTECEDENTES HEREDO-FAMILIARES\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14))
        {
            Alignment = Element.ALIGN_CENTER,
        };
        doc.Add(encabezado);

        //Primera fila
        _utilPdf.CellEdit(datosGenerales, " ", baseFont, 10, 5, 1, true, true, 5f, 0, 0, 0, 0, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Padres:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Vivos:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Fallecidos:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, "No hay informacion", fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Padres.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.PadresVivos.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Causa:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.PadresCausaMuerte, fontResponse, 10, 2, true);
        
        //Segunda fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Hermanos:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Vivos:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Fallecidos:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, "No hay informacion", fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Hermanos.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.HermanosVivos.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Causa:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.HermanosCausaMuerte, fontResponse, 10, 2, true);
        
        //Tercera fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Hijos:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Vivos:", baseFont, 14, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Fallecidos:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, "No hay informacion", fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Hijos.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.HijosVivos.ToString(), fontResponse, 10, 1, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Causa:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.HijosCausaMuerte, fontResponse, 10, 2, true);
        
        //Cuarta fila
        _utilPdf.AddCellWithHeigth(datosGenerales, "Enfermedades:", baseFont, 14, 2, true, false, 40f);
        _utilPdf.AddCellWithHeigth(datosGenerales, "Taxicomanias:", baseFont, 14, 2, true, false, 40f);
        
        //Quinta fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "DM:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Alcoholismo:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Dm, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Alcoholismo, fontResponse, 10, 2, true);
        
        //Sexto fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "HTA:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Tabaquismo:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Hta, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Tabaquismo, fontResponse, 10, 2, true);
        
        //Septima fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Cáncer:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Drogas:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Cancer, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, hf.Drogas, fontResponse, 10, 2, true);
        
        return datosGenerales;
    }
    
    public PdfPTable HistoriaClinica(Document doc, Domain.Entities.Diagnostico diag, Expediente exp, Paciente pac) {
        PdfPTable datosGenerales = new PdfPTable(4);
        
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 20;
        datosGenerales.SpacingAfter = 20;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph encabezado = new Paragraph("HISTORIA CLINICA FISIOTERAPIA\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14, Font.BOLD))
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 10f
        };
        doc.Add(encabezado);

        Paragraph datos = new Paragraph("DATOS PERSONALES\n", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14));
        doc.Add(datos);

        //Primera fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Número de expediente:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Fecha:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, exp.Nomenclatura, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, "25 Agosto 2024", fontResponse, 10, 2, true);
        
        //Segunda fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Diagnóstico:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Refiere:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, diag.Descripcion, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, diag.Refiere, fontResponse, 10, 2, true);
        
        //Tercera fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Nombre:", baseFont, 14, 4, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, $"{pac.Nombre} {pac.Apellido}", fontResponse, 10, 4, true);
        
        //Cuarta fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Domicilio:", baseFont, 14, 3, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "C.P:", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Domicilio, fontResponse, 10, 3, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.CodigoPostal.ToString(), fontResponse, 10, 1, true);
        
        //Quinta fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Edad:", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Sexo:", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Estado Civil:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, $"{FormatDate.DateToYear(pac.Edad)} años", fontResponse, 10, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Sexo ? "Hombre" :  "Mujer", fontResponse, 10, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.CatEstadoCivil.Descripcion, fontResponse, 10, 2, true);
        
        //Sexta fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Ocupación:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Tipo de interrogatorio:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Ocupacion, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, exp.TipoInterrogatorio ? "Directo" : "Indirecto", fontResponse, 10, 2, true);
        
        //Septima fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Teléfono / Celular:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Responsable en caso de menor o adulto mayor:", baseFont, 14, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Telefono, fontResponse, 10, 2, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, !exp.TipoInterrogatorio ? exp.Responsable : "", fontResponse, 10, 2, true);
        
        return datosGenerales;
    }
    
    public PdfPTable HojaFrontalP2(Domain.Entities.Diagnostico diag, Fisioterapeuta fisio) {
        PdfPTable datosGenerales = new PdfPTable(2);
        
        float[] headers = { 30, 70 }; 
        datosGenerales.SetWidths(headers);
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 25;
        
        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //Primera fila
        _utilPdf.AddCellWithHeigth(datosGenerales, "Fecha:", baseFont, 14, 1, true, true, 80f);
        _utilPdf.AddCellWithHeigth(datosGenerales, "25 agosto 2024", fontResponse, 10, 1, true, false, 80f); //diag.FechaInicio.ToString("dd MMMM yyyy", new CultureInfo("es-ES"))
        
        //Segunda fila
        _utilPdf.AddCellWithHeigth(datosGenerales, "Previo diagnóstico médico", baseFont, 14, 1, true, true, 80f);
        _utilPdf.AddCellWithHeigth(datosGenerales, diag.DiagnosticoPrevio, fontResponse, 10, 1, true, false, 80f);
        
        //Tercera fila
        _utilPdf.AddCellWithHeigth(datosGenerales, "Diagnóstico funcional", baseFont, 14, 1, true, true, 80f);
        _utilPdf.AddCellWithHeigth(datosGenerales, diag.DiagnosticoFuncional, fontResponse, 10, 1, true, false, 80f);
        
        //Cuarta fila
        _utilPdf.AddCellWithHeigth(datosGenerales, "Nombre del fisioterapeuta tratante", baseFont, 14, 1, true, true, 80f);
        _utilPdf.AddCellWithHeigth(datosGenerales, fisio.Nombre, fontResponse, 10, 1, true, false, 80f);

        return datosGenerales;
    }
    
    public PdfPTable HojaFrontal(Document doc, Expediente exp, Paciente pac) {
        PdfPTable datosGenerales = new PdfPTable(3);
        
        datosGenerales.WidthPercentage = 100;
        datosGenerales.HeaderRows = 1;
        datosGenerales.SpacingBefore = 0;

        BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        BaseFont fontResponse = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        
        //Agregamos su encabezado
        Paragraph uac = new Paragraph("\nHOJA FRONTAL", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14))
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 15f
        };
        doc.Add(uac);
        
        //Primera fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Nombre del cliente/paciente:", baseFont, 14, 3, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, $"{pac.Nombre} {pac.Apellido}", fontResponse, 10, 3, true);
        
        //Segunda fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "N° de expediente", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Edad:", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Sexo:", baseFont, 14, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, exp.Nomenclatura, fontResponse, 10, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, FormatDate.DateToYear(pac.Edad).ToString(), fontResponse, 10, 1, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Sexo ? "Hombre" : "Mujer", fontResponse, 10, 1, true);

        //Tercera fila
        _utilPdf.AddCellWithOutBorderButton(datosGenerales, "Institución:", baseFont, 14, 3, true);
        _utilPdf.AddCellWithOutBorderTop(datosGenerales, pac.Institucion, fontResponse, 10, 3, true);
        
        return datosGenerales;
    }

    public void Encabezado2(Document doc)
    {
        // Encabezado
        Paragraph uac = new Paragraph("UNIVERSIDAD AUTÓNOMA DE CAMPECHE " +
                                      "\nFACULTAD DE ENFERMERÍA" +
                                      "\nLICENCIATURA EN FISIOTERAPIA" +
                                      "\nHOJA DE ALTA", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14))
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingBefore = 0
        };
        doc.Add(uac);
    }
    
    public void Encabezado(Document doc)
    {
        // Encabezado
        Paragraph uac = new Paragraph("UNIVERSIDAD AUTÓNOMA DE CAMPECHE " +
                                      "\nFACULTAD DE ENFERMERÍA" +
                                      "\nLICENCIATURA EN FISIOTERAPIA" +
                                      "\nCLINICA FISIOTERAPEUTICA", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14))
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingBefore = 0
        };
        doc.Add(uac);
    }
}

public record ReporteExpedienteResponse
{
    public string Reporte { get; set; }
}