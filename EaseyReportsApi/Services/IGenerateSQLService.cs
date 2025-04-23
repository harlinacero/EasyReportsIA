using EaseyReportsApi.Dtos;

namespace EaseyReportsApi.Services
{
    public interface IGenerateSQLService
    {
        Task<ApiResponse> GenerateSQLData(string naturalQuery);
    }
}
