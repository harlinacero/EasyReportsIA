using EaseyReportsDomain.Entities;

namespace EaseyReportsDomain.Classes
{
    public interface IGenerateSQL
    {
        Task<Report> BuildSqlQuery(string naturalQuery);
    }
}