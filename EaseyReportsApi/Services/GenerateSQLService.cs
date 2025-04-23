using EaseyReportsApi.Dtos;
using EaseyReportsDomain.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EaseyReportsApi.Services
{
    public class GenerateSQLService : IGenerateSQLService
    {
        private readonly IGenerateSQL _generateSQL;

        public GenerateSQLService(IGenerateSQL generateSQL)
        {
            _generateSQL = generateSQL;
        }

        public async Task<ApiResponse> GenerateSQLData(string naturalQuery)
        {
            var response = await _generateSQL.BuildSqlQuery(naturalQuery);
            var apiResponse = new ApiResponse()
            {
                Data = response.Data.ToString(),
                StrSql = response.SqlQuery,
            };

            return apiResponse; 
        }
    }
}
