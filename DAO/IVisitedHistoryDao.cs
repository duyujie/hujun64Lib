using System.Web.UI.WebControls;
using System;
using System.Data;
using com.hujun64.po;
using com.hujun64.logic;
namespace com.hujun64.Dao
{
    /// <summary>
    ///ILinkDao ��ժҪ˵��
    /// </summary>
    public interface IVisitedHistoryDao  
    {
      
        ClickIpSummary SummaryClickIp(DateTime dateFrom,DateTime dateTo);
        DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo);
        DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo, int topRows);
        int InsertVisitedHistory(VisitedHistory visitedHistory);
       
    }
}