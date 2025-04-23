using EaseyReportsDomain.Entities;
using EaseyReportsDomain.Interfaces;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace EaseyReportsDomain.Classes
{
    public class GenerateSQL : IGenerateSQL
    {
        private readonly IConnectionModelIA _connectionModelIA;
        private readonly IRepository _repository;
        public GenerateSQL(IConnectionModelIA connectionModelIA, IRepository repository)
        {
            _connectionModelIA = connectionModelIA;
            _repository = repository;
        }

        public async Task<Report> BuildSqlQuery(string naturalQuery)
        {
            try
            {
                // Mapear el esquema de la base de datos
                var databaseScheme = await _repository.GetDataBaseScheme();
                // Armar el prompt
                var prompt = BuildIdealPromt(naturalQuery, databaseScheme);
                // Conectar con el modelo IA y obtener el resultado
                var query = await _connectionModelIA.GenerateSQlQueryString(prompt);
                if (query == null)
                {
                    throw new Exception("Ocurrió un error al interpretar el query");
                }

                var data = await _repository.ExecuteQuery(query);
                if (data == null)
                {
                    throw new Exception("Ocurrió un error a generar la consulta");
                }

                return new Report()
                {
                    Data = data,
                    SqlQuery = query
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private static string BuildIdealPromt(string naturalQuery, DataObjectModel databaseScheme)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Eres un asistente SQL experto. A partir de la petición del usuario en lenguaje natural y el esquema de base de datos proporcionado, ");
            stringBuilder.Append("tu única tarea es generar una **consulta SQL válida, limpia y lista para ejecutarse directamente**");
            stringBuilder.Append("⚠️ Muy importante:");
            stringBuilder.Append("- Devuelve **únicamente** la consulta SQL, sin explicaciones, sin comentarios, sin envoltorios de código (como ```sql), ni texto adicional.");
            stringBuilder.Append("- Asegúrate de que sea compatible con **SQL Server**.");
            stringBuilder.Append("- Si el requerimiento no se puede resolver exactamente, crea la consulta más razonable posible.");
            stringBuilder.Append("- Si no se proporciona el nombre de la tabla o campos exactos, haz suposiciones razonables y explícitas. ");
            stringBuilder.Append("- Si la instrucción es ambigua, devuelve una versión tentativa de la consulta con comentarios explicativos. ");
            stringBuilder.Append($"El esquema de la base de datos es: {JsonConvert.SerializeObject(databaseScheme)}");
            stringBuilder.Append("Descripción del usuario: ");
            stringBuilder.Append(naturalQuery);

            return stringBuilder.ToString();
        }
    }
}
