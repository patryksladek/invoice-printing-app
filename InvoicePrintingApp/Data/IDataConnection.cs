namespace InvoicePrintingApp.Data
{
    public interface IDataConnection
    {
        string GetDocumentNumberByInvoiceID(int ID);
    }
}
