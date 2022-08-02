namespace PdfGenerator.Exceptions;

public class FailToAccessPageExceptions : Exception
{
    public FailToAccessPageExceptions() : base("There was a problem accessing the page")
    {
    }
}