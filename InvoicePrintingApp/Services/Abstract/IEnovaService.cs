namespace InvoicePrintingApp.Services.Abstract
{
    public interface IEnovaService
    {
        void PrintInvoice(int invoiceID, string printPatternName, string path);
    }
}
