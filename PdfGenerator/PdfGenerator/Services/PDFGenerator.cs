using Microsoft.Extensions.Logging;
using PdfGenerator.Exceptions;
using PdfGenerator.Interfaces;
using PdfGenerator.Options;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PdfGenerator.Services;

public class PDFGenerator : IPDFGenerator
{
    private readonly ILogger<PDFGenerator> _logger;
    private readonly LaunchOptions _options;
    private Browser _browser;
    public PDFGenerator(ILogger<PDFGenerator> logger, LaunchOptions options)
    {
        _logger = logger;
        _options = options;
        _browser = Init(options).Result;
    }

    private async Task<Browser> Init(LaunchOptions options)
    {
        var info = await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        var browser = await Puppeteer.LaunchAsync(options);
        browser.Closed += HandleCloseBrowsers;
        
        _logger.LogInformation("Browser has launched and is ready");
        _logger.LogInformation("Chromium revision: {InfoRevision}", info.Revision);
        
        return browser;
    }
    
    public async Task<Stream> GetStreamAsync(string url, Action<PdfGeneratorOptions>? options = null)
    {
        await using var page = await _browser.NewPageAsync();
        try
        {
            var response = await page.GoToAsync(url);
            _logger.LogInformation("Accessing: {Url}", url);

            var pdfOptions = new PdfGeneratorOptions();
            options?.Invoke(pdfOptions);

            var stream = await page.PdfStreamAsync(ConfigureOptions(pdfOptions));

            if (!response.Ok)
            {
                throw new FailToAccessPageExceptions();
            }
            
            return stream;

        }
        finally
        {
            await page.CloseAsync();
        }
    }
    
    public async Task SavePdfAsync(string url, string file, Action<PdfGeneratorOptions>? options = null)
    {
        await using var page = await _browser.NewPageAsync();
        try
        {
            var response = await page.GoToAsync(url);
            _logger.LogInformation("Accessing: {Url}", url);

            var pdfOptions = new PdfGeneratorOptions();
            options?.Invoke(pdfOptions);

           await page.PdfAsync(file, ConfigureOptions(pdfOptions));

            if (!response.Ok)
            {
                throw new FailToAccessPageExceptions();
            }
            
        }
        finally
        {
            await page.CloseAsync();
        }
    }
    
    private void HandleCloseBrowsers(object? sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            await _browser.DisposeAsync();
            _browser = await Init(_options);
            _logger.LogInformation("Browser has restarted");
        });
    }

    private PdfOptions ConfigureOptions(PdfGeneratorOptions pdfOptions)
    {
        return new PdfOptions
        {
            Format = new PaperFormat(pdfOptions.PaperSize.Width, pdfOptions.PaperSize.Height),
            MarginOptions =
            {
                Top = pdfOptions.Margin.Top.ToString(),
                Bottom = pdfOptions.Margin.Bottom.ToString(),
                Left = pdfOptions.Margin.Left.ToString(),
                Right = pdfOptions.Margin.Right.ToString(),
            },
            DisplayHeaderFooter = true,
            HeaderTemplate = pdfOptions.Header,
            FooterTemplate = pdfOptions.Footer,
            PrintBackground = true
        };
    }
}