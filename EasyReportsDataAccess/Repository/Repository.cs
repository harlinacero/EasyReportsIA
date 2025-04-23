using EaseyReportsDomain.Entities;
using EaseyReportsDomain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace EasyReportsDataAccess.Repository
{
    public class Repository : IRepository
    {
        private const string CONN_STRING = "SqlConnectionString";
        private readonly string connectionString;

        public Repository(IConfiguration config)
        {
            connectionString = config.GetConnectionString(CONN_STRING);
        }

        public async Task<string> ExecuteQuery(string query)
        {
            var cleanSQL = query.Trim().Trim('`');
            var results = new List<Dictionary<string, object>>();
            using SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new(cleanSQL, connection))
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }

                        results.Add(row);
                    }
                }

                // Serializa la lista de resultados a JSON
                return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
               throw new Exception("Error al ejecutar la consulta: " + ex.Message);
            }
        }

        public async Task<DataObjectModel> GetDataBaseScheme()
        {
            var model = new DataObjectModel();
            try
            {
                using SqlConnection connection = new(connectionString);
                connection.Open();

                DataTable tables = await connection.GetSchemaAsync("Tables");

                foreach (DataRow table in tables.Rows)
                {
                    string tableSchema = table["TABLE_SCHEMA"].ToString();
                    string tableName = table["TABLE_NAME"].ToString();
                    Console.WriteLine($"\nTabla: {tableSchema}.{tableName}");
                    var newTable = new Table
                    {
                        Name = $"\nTabla: {tableSchema}.{tableName}",
                        Columns = new List<Column>()
                    };

                    // Obtener columnas de la tabla actual
                    DataTable columns = await connection.GetSchemaAsync("Columns", new string[] { null, tableSchema, tableName });

                    foreach (DataRow column in columns.Rows)
                    {
                        string columnName = column["COLUMN_NAME"].ToString();
                        string dataType = column["DATA_TYPE"].ToString();
                        Console.WriteLine($" - Columna: {columnName}, Tipo: {dataType}");
                        newTable.Columns.Add(new Column
                        {
                            ColumnName = columnName,
                            DataType = dataType
                        });
                    }

                    model.Tables.Add(newTable);
                }

                connection.Close();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el esquema: " + ex.Message);
            }
        }
    }
}
