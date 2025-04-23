using EaseyReportsDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EaseyReportsDomain.Interfaces
{
    public interface IConnectionModelIA
    {
        Task<string> GenerateSQlQueryString(string prompt);
    }
}
