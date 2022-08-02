using PuppeteerSharp.Media;

namespace PdfGenerator.Options;

public class PaperSize
{
    public decimal Width { get; }
    public decimal Height { get; }

    public PaperSize(decimal width, decimal height)
    {
        Width = width;
        Height = height;
    }

    public PaperSize(PaperFormat paperFormat, bool landscape = false) : this( !landscape ? paperFormat.Width : paperFormat.Height, !landscape ? paperFormat.Height : paperFormat.Width)
    {
    }
}