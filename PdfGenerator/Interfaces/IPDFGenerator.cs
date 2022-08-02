
using PdfGenerator.Options;

namespace PdfGenerator.Interfaces;

public interface IPDFGenerator
{
    Task<Stream> GetStreamAsync(string url, Action<PdfGeneratorOptions>? options = null);
    Task SavePdfAsync(string url, string file, Action<PdfGeneratorOptions>? options = null);
}