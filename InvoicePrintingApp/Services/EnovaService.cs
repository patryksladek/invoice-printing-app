using InvoicePrintingApp.Data;
using InvoicePrintingApp.Helpers;
using InvoicePrintingApp.Services.Abstract;
using InvoicePrintingApp.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Soneta.Business;
using Soneta.Business.App;
using Soneta.Business.UI;
using Soneta.Handel;
using Soneta.Start;
using System;
using System.IO;

namespace InvoicePrintingApp.Services
{
    public class EnovaService : IEnovaService
    {
        private readonly IDataConnection _dataConnection;
        private readonly ILogger<EnovaService> _log;
        private readonly EnovaSettings _enovaSettings;

        private readonly Login _login;

        public EnovaService(IDataConnection dataConnection, IOptions<EnovaSettings> enovaSettings, ILogger<EnovaService> log)
        {
            _dataConnection = dataConnection;
            _enovaSettings = enovaSettings.Value;
            _log = log;

            _login = Login();
        }

        private Login Login()
        {
            // Automatyczne zarejestrowanie i zainicjowanie bibliotek 
            Loader loader = new Loader();
            loader.WithExtensions = true;
            loader.Load();

            // Uzyskanie dostęp do obiektu bazy danych 
            Database database = BusApplication.Instance[_enovaSettings.Instance];

            // Uzyskanie loginu do bazy danych, czyli zalogowanie się operatora
            Login login = database.Login(false, _enovaSettings.User, _enovaSettings.Password);
                
            return login;
        }

        public void PrintInvoice(int invoiceID, string printPatternName, string path)
        {
            string invoiceNumber = _dataConnection.GetDocumentNumberByInvoiceID(invoiceID);
            if (invoiceNumber == null)
                throw new Exception("Nie znaleziono faktury o podanym identyfikatorze.");

            using (Session session = _login.CreateSession(false, false))
            {
                HandelModule hm = HandelModule.GetInstance(session);
                DokumentHandlowy dok = hm.DokHandlowe.NumerWgNumeruDokumentu[invoiceNumber];

                string filepath = Path.Combine(path, FileHelper.GenerateFileName(dok));

                Context context = Context.Empty.Clone(session);

                context.Set(dok);
                context.Set(new ParametryWydrukuDokumentu(context) 
                { 
                    Oryginał = true, 
                    Duplikat = false, 
                    IloscKopii = 0 
                });

                //TODO: Nieudana próba utworzenia serwisu Soneta.Business.UI.IReportService - do poprawienia.
                IReportService service;
                BusApplication.Instance.GetService(out service);

                ReportResult reportResult = new ReportResult()
                {
                    Context = context,
                    Preview = false,
                    Format = ReportResultFormat.PDF,

                    TemplateFileSource = AspxSource.Storage,
                    TemplateFileName = printPatternName,
                };

                var streamW = service.GenerateReport(reportResult);

                FileHelper.SaveOnDisk(filepath, path, streamW);

                Console.WriteLine("Operacja zakończyła się powodzeniem.");
            }
        }
    }
}
