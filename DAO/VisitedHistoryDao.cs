using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.WebControls;
using com.hujun64.po;
using com.hujun64.logic;
using com.hujun64.util;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    ///VisitedHistoryDao 的摘要说明
    /// </summary>
    public class VisitedHistoryDao  : AdoDaoSupport, IVisitedHistoryDao
    {
        
       
        public  ClickIpSummary SummaryClickIp(DateTime dateFrom,DateTime dateTo)
        {
            ClickIpSummary clickSummary = new ClickIpSummary();
            clickSummary.dateFrom = dateFrom;
            clickSummary.dateTo = dateTo;
           
 
                StringBuilder sb=new StringBuilder("select count(1) as click,count(distinct v.ip_from) as ip, count(distinct case when f.ip_province like '上海%' then f.ip_from else null end) as ip_shanghai ");
                sb.Append(" from  dbo.view_visited_history v left outer join ip_info f ");
                sb.Append(" on v.ip_from = f.ip_from ");
                sb.Append(" where v.visited_time >= dbo.getdateZero(@dateFrom) and v.visited_time < dbo.getdateZero(@dateTo+1)");
                IDbParameters dbParameters = CreateDbParameters();

                dbParameters.AddWithValue("dateFrom", dateFrom);
                dbParameters.AddWithValue("dateTo", dateTo);

                

                clickSummary.days = Util.DateDiff(dateTo, dateFrom).Days;
                DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sb.ToString(), dbParameters);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    clickSummary.click = (int)row["click"];
                    clickSummary.ip = (int)row["ip"];
                    clickSummary.ip_shanghai = (int)row["ip_shanghai"];
                }

             

                return clickSummary;
           
        }
        public DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo)
        {
            ClickSourceSummary clickSummary = new ClickSourceSummary();
            clickSummary.dateFrom = dateFrom;
            clickSummary.DateTo = dateTo;
 
                StringBuilder sb=new StringBuilder("select count(1) as click,count(distinct ip_from) as ip, case click_source_site when '' then '直接访问' else click_source_site end as click_source");
                sb.Append(" from  dbo.visited_history ");
                sb.Append(" where visited_time >= dbo.getdateZero(@dateFrom) and visited_time < dbo.getdateZero(@dateTo+1)");
                sb.Append(" and click_source_site is not null  group by click_source_site order by click desc");
                IDbParameters dbParameters = CreateDbParameters();

                dbParameters.AddWithValue("dateFrom", dateFrom);
                dbParameters.AddWithValue("dateTo", dateTo);

                return AdoTemplate.DataSetCreateWithParams(CommandType.Text, sb.ToString(), dbParameters);

           
        }
        public DataSet SummaryClickSource(DateTime dateFrom, DateTime dateTo, int topRows)
        { 
                StringBuilder sb=new StringBuilder("select top ");
                sb.Append(topRows);
                sb.Append(" c.*,i.ip from ");
                sb.Append(" (select page_class+isnull(page_id,'') as id,page_class,isnull(page_id,page_class) as page_id,page_title,count(1) as click from  dbo.view_visited_history where visited_time >=dbo.getdateZero(@dateFrom) and visited_time <dbo.getdateZero(@dateTo+1) group by page_class,page_id,page_title) c,");
                sb.Append(" (select count(distinct ip_from) as ip,page_class,isnull(page_id,page_class) as page_id,page_title from dbo.view_visited_history where visited_time >=dbo.getdateZero(@dateFrom) and  visited_time <dbo.getdateZero(@dateTo+1) group by page_class,page_id,page_title) i");
                sb.Append(" where c.page_class=i.page_class and c.page_id=i.page_id");
                sb.Append(" order by click desc,ip desc");


                IDbParameters dbParameters = CreateDbParameters();
                dbParameters.AddWithValue("dateFrom", dateFrom);
                dbParameters.AddWithValue("dateTo", dateTo);


                return AdoTemplate.DataSetCreateWithParams(CommandType.Text, sb.ToString(), dbParameters);
            

        }
        public int InsertVisitedHistory(VisitedHistory visitedHistory)
        { 
                string sql = "insert into visited_history (id,page_type,page_id,ip_from,visited_time,click_source_site,click_source_url) values (@visited_id,@page_type,@page_id,@ip_from,getdate(),@click_source_site,@click_source_url);";
                IDbParameters dbParameters = CreateDbParameters();


                dbParameters.AddWithValue("visited_id", visitedHistory.id);
                dbParameters.AddWithValue("page_type", visitedHistory.page_type);
                dbParameters.AddWithValue("click_source_site", visitedHistory.click_source_site);
                dbParameters.AddWithValue("click_source_url", visitedHistory.click_source_url);
                if (string.IsNullOrEmpty(visitedHistory.page_id))
                {
                    dbParameters.AddWithValue("page_id", DBNull.Value);
                }
                else
                {
                    dbParameters.AddWithValue("page_id", visitedHistory.page_id);
                }
                dbParameters.AddWithValue("ip_from", visitedHistory.ip_from);


              
                return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
            
        }
    }
}