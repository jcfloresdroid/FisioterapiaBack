using Core.Services.Interfaz;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Core.Services.Implementacion;

public class UtilPdf : IUtilPdf
{
    public void AddCell(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
        };
        
        tableLayout.AddCell(cell);
    }

    public void AddCellWithHeigth(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft, bool lTop, float heigth) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            VerticalAlignment = lTop ? Element.ALIGN_TOP : Element.ALIGN_MIDDLE,
            Colspan = Colspan,
            FixedHeight = heigth,
            PaddingLeft = 7f,
            PaddingTop = 7f,
            PaddingRight = 7f,
            PaddingBottom = 7f,
        };
        
        tableLayout.AddCell(cell);
    }

    public void AddCellWithHeigthTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft, bool lTop, float heigth) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            VerticalAlignment = lTop ? Element.ALIGN_TOP : Element.ALIGN_MIDDLE,
            Colspan = Colspan,
            FixedHeight = heigth,
            PaddingLeft = 7f,
            PaddingTop = 7f,
            PaddingRight = 7f,
            PaddingBottom = 7f,
            BorderWidthTop = 0
        };
        
        tableLayout.AddCell(cell);
    }

    public void AddCellWithOutBorder(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft)
    {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
            Border = PdfPCell.NO_BORDER,
        };
        
        tableLayout.AddCell(cell);
    }

    public void AddCellWithOutBorderButton(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
            PaddingLeft = 7f,
            PaddingTop = 2f,
            PaddingRight = 7f,
            PaddingBottom = 3f,
        };
        
        cell.BorderWidthBottom = 0;
        tableLayout.AddCell(cell);
    }

    public void AddCellWithOutBorderButton(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
            Rowspan = RowSpan,
            PaddingLeft = 7f,
            PaddingTop = 2f,
            PaddingRight = 7f,
            PaddingBottom = 3f,
        };
        
        cell.BorderWidthBottom = 0;
        tableLayout.AddCell(cell);
    }

    public void AddCellWithBorder(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan,
        bool lLeft)
    {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            Colspan = Colspan,
            Rowspan = RowSpan,
            PaddingLeft = 7f,
            PaddingTop = 2f,
            PaddingRight = 7f,
            PaddingBottom = 3f,
        };
        
        tableLayout.AddCell(cell);
    }

    public void AddCellWithOutBorderTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
            PaddingLeft = 7f,
            PaddingTop = 3f,
            PaddingRight = 7f,
            PaddingBottom = 7f,
        };
        
        cell.BorderWidthTop = 0;
        tableLayout.AddCell(cell);
    }

    public void AddCellWithOutBorderTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_RIGHT,
            Colspan = Colspan,
            Rowspan = RowSpan,
            PaddingLeft = 7f,
            PaddingTop = 3f,
            PaddingRight = 7f,
            PaddingBottom = 7f,
        };
        
        cell.BorderWidthTop = 0;
        tableLayout.AddCell(cell);
    }

    public void CellEdit(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft, bool lTop, float heigth, int t, int r, int b, int l, bool pos) {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = lLeft ? Element.ALIGN_LEFT : Element.ALIGN_MIDDLE,
            VerticalAlignment = lTop ? Element.ALIGN_TOP : Element.ALIGN_MIDDLE,
            Colspan = Colspan,
            Rowspan = RowSpan,
            FixedHeight = heigth,
            PaddingLeft = 7f,
            PaddingTop = pos ? 2f : 3f,
            PaddingRight = 7f,
            PaddingBottom = pos ? 3f : 7f,
            BorderWidthTop = t,
            BorderWidthRight = r,
            BorderWidthBottom = b,
            BorderWidthLeft = l,
        };

        tableLayout.AddCell(cell);
    }

    public void Revisiones(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan)
    {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            Colspan = Colspan,
            Border = PdfPCell.NO_BORDER,
        };
    
        tableLayout.AddCell(cell);
    }

    public void ExploracionF(PdfPTable tableLayout, string cellText, BaseFont baseFont, int fontSize, int colspan, bool padding)
    {
        BaseColor textColor = new BaseColor(107,114,128); 
        Font coloredFont = new Font(baseFont, fontSize) { Color = textColor };

        PdfPCell cell = new PdfPCell(new Phrase(cellText, coloredFont))
        {
            HorizontalAlignment = Element.ALIGN_CENTER,
            BackgroundColor = new BaseColor(235, 245, 255),
            Colspan = colspan,
            Border = PdfPCell.NO_BORDER,
            PaddingBottom = padding ? 10f : 0f,
        };

        tableLayout.AddCell(cell);
    }

    public void CellEdits(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int tamano, bool ca, bool cl, List<bool> border)
    {
        PdfPCell cell = new PdfPCell(new Phrase(cellText, new Font(Font, FontSize)))
        {
            HorizontalAlignment = cl ? Element.ALIGN_LEFT : Element.ALIGN_CENTER,
            VerticalAlignment = ca ? Element.ALIGN_TOP : Element.ALIGN_MIDDLE,
            Colspan = Colspan,
            FixedHeight = tamano,
            BorderWidthTop = border[0] ? 1 : 0,
            BorderWidthRight =  border[1] ? 1 : 0,
            BorderWidthBottom =  border[2] ? 1 : 0,
            BorderWidthLeft =  border[3] ? 1 : 0,
            PaddingLeft = 7f,
            PaddingRight = 7f
        };
        
        tableLayout.AddCell(cell);
    }
}