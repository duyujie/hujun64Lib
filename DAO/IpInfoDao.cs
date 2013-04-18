using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Spring.Data.Core;
using Spring.Data.Common;
using System.Text;

namespace com.hujun64.Dao
{
    /// <summary>
    ///GuestbookDao 的摘要说明
    /// </summary>
    public class IpInfoDao : AdoDaoSupport, IIpInfoDao
    {
       
        public int InsertIp(string ipFrom)
        { 
                string sql = " if not exists (select 1 from ip_info where ip_from=@ipFrom)	insert into ip_info (ip_from) values (@ipFrom)";

                IDbParameters dbParameters = CreateDbParameters();
                dbParameters.AddWithValue("ipFrom", ipFrom);

                int i = AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
                return i < 0 ? 0 : i;
 
        }
        public int UpdateIpProvince(string ipFrom, string ipProvince)
        {
            if (string.IsNullOrEmpty(ipProvince))
                return 0;

             
                string sql = "update ip_info set ip_province=@ip_province where ip_from=@ip_from";

                IDbParameters dbParameters = CreateDbParameters();

                dbParameters.AddWithValue("ip_from", ipFrom);
                dbParameters.AddWithValue("ip_province", ipProvince);

                
                return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

            
        }
        public int FixIpFrom()
        { 
                
                StringBuilder sqlSb =new StringBuilder( "insert into ip_info (ip_from) select distinct ip_from from guestbook g where not exists (select 1 from ip_info i where g.ip_from=i.ip_from)");
                sqlSb.Append(" update ip_info set ip_province=null where ip_province like '%联系我们%'");
           
                return AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlSb.ToString());
                

           
        
        }
        public List<string> GetQueryIpFromList()
        { 
                string sql = "select ip_from from ip_info where ip_province is null";

                IDbParameters dbParameters = CreateDbParameters();
                          

                
                List<string> list = new List<string>();
                DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(row["ip_from"].ToString());
                }
           
                return list;

            
        }
    }
}