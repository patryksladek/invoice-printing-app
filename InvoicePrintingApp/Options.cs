using CommandLine;

namespace InvoicePrintingApp
{
    public class Options
    {
        [Option("id", Required = true, HelpText = "Identyfikator faktury.")]
        public int InvoiceID { get; set; }

        [Option("pattern", Required = true, HelpText = "Nazwy wzorca wydruku.")]
        public string PrintPatternName { get; set; }

        [Option("path", Required = true, HelpText = "Ścieżka do folderu, w którym zostanie zapisany plik PDF.")]
        public string Path { get; set; }
    }
}
