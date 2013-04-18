using System;
using System.Data;

namespace com.hujun64
{
    /// <summary>
    ///UtilData 的摘要说明
    /// </summary>
    public class UtilData
    {
        public UtilData()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// datatable分页方法，和其他的方法比较优势为不需要datatable.copy()操作，
        /// 节约了一定的资源。by adchun
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="curpageno"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="maxPageNo"></param>
        /// <param name="prePageNo"></param>
        /// <param name="nextPageNo"></param>
        public static DataTable GetPagedTable
        (
        DataTable dataTable,	//要分页的表
         int currentPageNo,	//当前页码，从1开始
        int pageSize	//每页记录数
      

        )
        {
             //以下这句为防止curpageno出界，可有可无
            currentPageNo = currentPageNo < 1 ? currentPageNo = 1 : currentPageNo ;
          
            //从上往下删除表中不需要的记录
            int cyctimes = (currentPageNo - 1) * pageSize;
            for (int i = 0; (dataTable.Rows.Count > 0 && i < cyctimes); i++)
            {
                dataTable.Rows.RemoveAt(0);
            }
            //从下往上删除表中不需要的记录，i值减少的同时也意味着rows.count值的减少
            for (int i = dataTable.Rows.Count; i > pageSize; i--)
            {
                dataTable.Rows.RemoveAt(i - 1);
            }

            return dataTable;
        }
        public static int GetColumnIndexOfTable(DataTable table, string columnCaption){

            int i = 0;
            foreach (DataColumn column in table.Columns)
            {
                if (column.Caption.Equals(columnCaption))
                {
                    return i;
                }
                i++;
            }

            return -1;
        }
    }
}