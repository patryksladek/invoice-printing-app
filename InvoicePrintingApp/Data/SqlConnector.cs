using Dapper;
using System.Data.SqlClient;

namespace InvoicePrintingApp.Data
{
    public class SqlConnector : IDataConnection
    {
        public string ConnectionString { get; private set; }

        public SqlConnector(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string GetDocumentNumberByInvoiceID(int ID)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string sql = $"SELECT NumerPelny FROM DokHandlowe WHERE ID = {ID}";

                string documentNumber = connection.ExecuteScalar<string>(sql);

                return documentNumber;
            }
        }
    }
}
