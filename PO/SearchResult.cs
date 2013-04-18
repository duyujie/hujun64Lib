using System;
using System.Data;
using System.Collections.Generic;

using com.hujun64.type;

namespace com.hujun64.po
{
    /// <summary>
    ///Client 的摘要说明
    /// </summary>
    public class SearchResult 
    {
        public int ResultCount=0;
        public DataSet ResultDataSet=null;

        private List<object> primaryKeyList = new List<object>();
        public SearchResult()
        {
        }
        public SearchResult(DataSet ds)
        {
            this.ResultDataSet =  ds;
            this.ResultCount = ds.Tables[0].Rows.Count;
             
        }

        public SearchResult( DataSet ds,int count)
        {
            this.ResultCount = count;
            this.ResultDataSet = ds;
         

        }


        public List<object> GetPrimaryKeyList(string  primaryKeyCaption)
        {
            
            DataTable table=ResultDataSet.Tables[0];


            int primaryKeyColumnIndex = UtilData.GetColumnIndexOfTable(table, primaryKeyCaption);
            if (primaryKeyColumnIndex >= 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    primaryKeyList.Add(row[primaryKeyColumnIndex]);
                }
            }

            return primaryKeyList;
        }

        public void Merge(SearchResult result, string primaryKeyCaption)
        {
            if (result == null)
                return;

            if (ResultDataSet == null)
            {
                ResultDataSet = result.ResultDataSet;
                ResultCount = result.ResultCount;
                return;
            }

            
            DataSet ds = result.ResultDataSet;
            GetPrimaryKeyList(primaryKeyCaption);
            DataTable table=ds.Tables[0];

            List<DataRow> deleteRowIndexList = new List<DataRow>();
            int primaryKeyColumnIndex = UtilData.GetColumnIndexOfTable(table, primaryKeyCaption);
            foreach (DataRow row in table.Rows)
            {
               
                foreach (object pk in primaryKeyList){
                    if (pk.ToString() == row[primaryKeyColumnIndex].ToString() && pk.GetHashCode() == row[primaryKeyColumnIndex].GetHashCode())
                    {
                        deleteRowIndexList.Add(row);
                    }
                 }
                
            }

            foreach(DataRow row in deleteRowIndexList){
                row.Delete();
            }
            table.AcceptChanges();
           
            ResultDataSet.Tables[0].Merge(table.Copy());
            ResultCount = ResultDataSet.Tables[0].Rows.Count;
          
           
        }
    }
}