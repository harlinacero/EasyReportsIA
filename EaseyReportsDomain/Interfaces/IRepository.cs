using EaseyReportsDomain.Entities;

namespace EaseyReportsDomain.Interfaces
{
    public interface IRepository
    {
        Task<DataObjectModel> GetDataBaseScheme();
        Task<string> ExecuteQuery(string query);
    }
}
