using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EaseyReportsDomain.Entities
{
    public class Report
    {
        public string SqlQuery { get; set; }
        public object Data { get; set; }
    }

    public class DataObjectModel
    {
        public List<Table> Tables { get; set; }

        public DataObjectModel() { 
            Tables = new List<Table>();
        }
    }

    public class Table
    {
        public string Name { get; set; }
        public List<Column> Columns { get; set; }

        public Table()
        {
            Columns = new List<Column>();
        }
    }

    public class Column
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
