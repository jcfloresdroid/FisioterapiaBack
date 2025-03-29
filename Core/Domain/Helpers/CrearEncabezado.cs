using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Core.Domain.Helpers;

public class CrearEncabezado : PdfPageEventHelper
{
    public CrearEncabezado()
    {
    }
    
    private static string rutaBase = Directory.GetCurrentDirectory();
    private static string uac = Path.Combine(rutaBase, "wwwroot", "Logos", "uac.png");
    private static string enf = Path.Combine(rutaBase, "wwwroot", "Logos", "Enfermeria.png");

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        PdfContentByte cb = writer.DirectContent;
        
        // Set up the image
        Image img = Image.GetInstance(uac);
        Image img2 = Image.GetInstance(enf);
        
        img.ScaleAbsolute(70, 70);
        img.SetAbsolutePosition(document.LeftMargin, document.PageSize.Height - 90);

        img2.ScaleAbsolute(70, 70);
        img2.SetAbsolutePosition(document.PageSize.Width - 80, document.PageSize.Height - 85);
        
        // Add the image to the document
        cb.AddImage(img);
        cb.AddImage(img2);
    }
}
