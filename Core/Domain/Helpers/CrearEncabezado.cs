using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Core.Domain.Helpers;

public class CrearEncabezado : PdfPageEventHelper
{
    public CrearEncabezado()
    {
    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        PdfContentByte cb = writer.DirectContent;
        
        // Set up the image
        Image img = Image.GetInstance("https://res.cloudinary.com/doi0znv2t/image/upload/v1724549566/Utils/UACAM.png");
        Image img2 = Image.GetInstance("https://res.cloudinary.com/doi0znv2t/image/upload/v1724549566/Utils/Enfermeria.png");
        
        img.ScaleAbsolute(251, 35);
        img.SetAbsolutePosition(document.LeftMargin, document.PageSize.Height - 65);

        img2.ScaleAbsolute(70, 70);
        img2.SetAbsolutePosition(document.PageSize.Width - 80, document.PageSize.Height - 85);
        
        // Add the image to the document
        cb.AddImage(img);
        cb.AddImage(img2);
    }
}
