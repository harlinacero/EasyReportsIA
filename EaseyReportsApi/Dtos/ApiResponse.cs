using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EaseyReportsApi.Dtos
{
    public class ApiResponse
    {
        public string Data { get; set; }
        public string StrSql { get; set; }
    }

    public class ApiRequest
    {
        public string NaturalQuery { get; set; }
    }
}
