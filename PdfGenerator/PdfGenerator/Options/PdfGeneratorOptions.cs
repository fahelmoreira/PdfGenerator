using PuppeteerSharp.Media;

namespace PdfGenerator.Options;

public class PdfGeneratorOptions
{
    public PaperSize PaperSize { get; init; } = new PaperSize(PaperFormat.A4);
    public PdfMargins Margin { get; init; } = new PdfMargins(10, 10, 10, 10);
    public string Header { get; init; }
    public string Footer { get; init; }

    public bool DisplayHeaderFooter { get; set; } = true;

    public bool PrintBackground { get; set; } = true;
}