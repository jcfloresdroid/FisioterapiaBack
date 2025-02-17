using iTextSharp.text.pdf;

namespace Core.Services.Interfaz;

public interface IUtilPdf
{
    void AddCell(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft);
    
    void AddCellWithHeigth(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft, bool lTop, float heigth);
    
    void AddCellWithHeigthTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft, bool lTop, float heigth);
    
    void AddCellWithOutBorder(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft);
    
    void AddCellWithOutBorderButton(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft);
    void AddCellWithOutBorderButton(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft);
    void AddCellWithBorder(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft);
    
    void AddCellWithOutBorderTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool lLeft);
    void AddCellWithOutBorderTop(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft);

    void CellEdit(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int RowSpan, bool lLeft, bool lTop, float heigth, int t, int r, int b, int l, bool pos);
    void Revisiones(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan);    
    // Revisiones
    void ExploracionF(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, bool Padding);  
    void CellEdits(PdfPTable tableLayout, string cellText, BaseFont Font, int FontSize, int Colspan, int tamano, bool ca, bool cl, List<bool> border);
}