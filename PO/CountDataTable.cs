using System.Data;
using System.Data.SqlClient;

namespace com.hujun64.po
{
    /// <summary>
    ///CountReader 的摘要说明
    /// </summary>
    public  class CountDataTable
    {
        public DataTable dataTable;
        public int count;
        public CountDataTable()
        {
           
        }
        public CountDataTable(DataTable dataTable)
        {
            this.dataTable = dataTable;
            this.count = 0;
        }
        public CountDataTable(DataTable dataTable, int count)
        {
            this.dataTable = dataTable;
            this.count = count;
        }
        public CountDataTable(SqlDataReader dataReader)
        {
            DataTable table = new DataTable();
            table.Load(dataReader);

            this.dataTable = table;
            this.count = 0;
        }
        public CountDataTable(SqlDataReader dataReader, int count)
        {
            DataTable table = new DataTable();
            table.Load(dataReader);

            this.dataTable = table;
            this.count = count;
        }
        public CountDataTable(DataSet dataSet)
        {

            this.dataTable = dataSet.Tables[0];
            this.count = 0;
        }
        public CountDataTable(DataSet dataSet, int count)
        {
           
            this.dataTable = dataSet.Tables[0];
            this.count = count;
        }
    }
}
