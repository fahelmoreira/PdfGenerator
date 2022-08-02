using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PdfGenerator.Interfaces;
using PdfGenerator.Services;
using PuppeteerSharp;

namespace PdfGenerator.Extensions;

public static class PDFGeneratorServiceExtensions
{
    public static void AddPDFGenerator(this IServiceCollection services, Action<LaunchOptions>? options = null)
    {
        var opt = new LaunchOptions
        {
            Headless = true,
            IgnoreHTTPSErrors = true,
            Devtools = false,
            LogProcess = true,
            Args = new [] { "--no-sandbox", "--disable-setuid-sandbox" }
        };
        options?.Invoke(opt);
        services.AddSingleton<IPDFGenerator>(s => new PDFGenerator(s.GetRequiredService<ILogger<PDFGenerator>>(), opt));
    }
}