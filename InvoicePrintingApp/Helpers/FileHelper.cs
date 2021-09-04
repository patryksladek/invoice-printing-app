using Soneta.Handel;
using Soneta.Tools;
using System;
using System.IO;

namespace InvoicePrintingApp.Helpers
{
    public class FileHelper
    {
        public static void SaveOnDisk(string filepath, string directory, Stream streamW)
        {
            Directory.CreateDirectory(directory);

            var file = File.Create(filepath);

            streamW.Seek(0, SeekOrigin.Begin);

            CoreTools.StreamCopy(streamW, file);

            file.Flush();

            if (!File.Exists(filepath))
                throw new Exception("Nie udało się wygenerować faktury.");
        }

        public static string GenerateFileName(DokumentHandlowy dok)
        {
            string code = dok.Kontrahent.Kod;
            string fvid = string.Join("", dok.NumerPelnyZapisany.Split(Path.GetInvalidFileNameChars()));
            string dt = DateTime.Now.ToString("ddMMyyyy");
            string ext = ".pdf";

            var fileName = string.Format($"{code}_{fvid}_{dt}{ext}");

            return fileName;
        }
    }
}
