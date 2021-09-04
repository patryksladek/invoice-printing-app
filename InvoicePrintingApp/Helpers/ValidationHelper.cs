using InvoicePrintingApp.Settings;
using Microsoft.Extensions.Configuration;

namespace InvoicePrintingApp.Helpers
{
    public class ValidationHelper
    {
        public static ValidateResult ValidateAppSettings(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DbConnectionString");
            if (string.IsNullOrEmpty(connectionString))
                return ValidateResult.Fail;

            var enovaSettings = new EnovaSettings();
            config.GetSection("EnovaSettings").Bind(enovaSettings);
            if (string.IsNullOrEmpty(enovaSettings.Instance) || string.IsNullOrEmpty(enovaSettings.User))
                return ValidateResult.Fail;

            return ValidateResult.Success;
        }
    }

    public enum ValidateResult
    {
        Fail,
        Success
    }
}
