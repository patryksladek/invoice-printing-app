using CommandLine;
using InvoicePrintingApp.Data;
using InvoicePrintingApp.Helpers;
using InvoicePrintingApp.Services;
using InvoicePrintingApp.Services.Abstract;
using InvoicePrintingApp.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace InvoicePrintingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var result = ValidationHelper.ValidateAppSettings(configuration);
            if (result == ValidateResult.Fail)
            {
                Console.WriteLine($"Brak lub błędne wartości parametrów konfiguracji.");
                Environment.Exit(0);
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IEnovaService, EnovaService>();
                    services.AddTransient<IDataConnection>((sp) => new SqlConnector(configuration.GetConnectionString("DbConnectionString")));
                    services.Configure<EnovaSettings>(configuration.GetSection(nameof(EnovaSettings)));
                })
                .UseSerilog()
                .Build();

            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed(o =>
                   {
                       try
                       {
                           var svc = ActivatorUtilities.CreateInstance<EnovaService>(host.Services);
                           svc.PrintInvoice(o.InvoiceID, o.PrintPatternName, o.Path);
                       }
                       catch (Exception ex)
                       {
                           Console.WriteLine($"Operacja zakończona niepowodzeniem. Szczegóły: {ex.Message}");
                           Log.Logger.Error($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                           Environment.Exit(0);
                       }
                   });
        }
    }
}
