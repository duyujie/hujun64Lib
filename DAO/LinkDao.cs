using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    ///LinkDao  的摘要说明
    /// </summary>
    public class LinkDao : AdoDaoSupport, ILinkDao
    {

        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {
            string sql = "update link set is_static=@isStatic  where is_static <> @isStatic and addtime<=@timestamp and site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("isStatic", Convert.ToInt32(isStatic));
            dbParameters.AddWithValue("timestamp", timestamp);
            dbParameters.AddWithValue("site_id", Total.SiteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp)
        {
            List<string> updatedIdList = new List<string>(idList.Count);


            string sql = "update link set is_static=@isStatic  where is_static <> @isStatic and addtime<=@timestamp and site_id=@site_id and id=@linkId";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("isStatic", Convert.ToInt32(isStatic));
            dbParameters.AddWithValue("timestamp", timestamp);
            dbParameters.AddWithValue("site_id", Total.SiteId);
         
            dbParameters.Add("linkId", SqlDbType.Char);
            foreach (string id in idList)
            {
                dbParameters.SetValue("linkId", id);
                if (AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters) > 0)
                {
                    updatedIdList.Add(id);
                }
            }
            return updatedIdList;


        }
        public Dictionary<string, DateTime> GetAllLinkIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> linkIdDict = new Dictionary<string, DateTime>();


            string sql = "select id,last_mod from link where site_id=@site_id";
            if (!isRefreshAll)
            {
                sql += " and is_static=0";
            }
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                linkIdDict.Add(row["id"].ToString(), Convert.ToDateTime(row["last_mod"]));
            }

            return linkIdDict;

        }

        private Link CloneFromReader(SqlDataReader reader)
        {
            Link link = new Link();


            link.link_id = reader["id"].ToString();
            link.link_site_name = reader["site_name"].ToString();
            link.link_site_url = reader["site_url"].ToString();
            link.link_site_logo = reader["site_logo"].ToString();
            link.link_description = reader["description"].ToString();
            link.my_url = reader["my_url"].ToString();
            link.addtime = (DateTime)reader["addtime"];
            link.last_mod = (DateTime)reader["last_mod"];
            if (reader["approve_time"] != DBNull.Value)
                link.approve_time = (DateTime)reader["approve_time"];
            link.approve_status = (ApproveStatus)reader["approve_status"];
            link.enabled = Convert.ToBoolean(reader["enabled"]);
            link.sort_seq = (int)reader["sort_seq"];


            return link;

        }
        private Link CloneFromDataRow(DataRow row)
        {
            Link link = new Link();


            link.link_id = row["id"].ToString();
            link.link_site_name = row["site_name"].ToString();
            link.link_site_url = row["site_url"].ToString();
            link.link_site_logo = row["site_logo"].ToString();
            link.link_description = row["description"].ToString();
            link.my_url = row["my_url"].ToString();
            link.addtime = (DateTime)row["addtime"];
            link.last_mod = (DateTime)row["last_mod"];
            if (row["approve_time"] != DBNull.Value)
                link.approve_time = (DateTime)row["approve_time"];
            link.approve_status = (ApproveStatus)row["approve_status"];
            link.enabled = Convert.ToBoolean(row["enabled"]);
            link.sort_seq = (int)row["sort_seq"];
            link.site_id = row["site_id"].ToString();


            return link;

        }
        public Link GetLink(string linkId)
        {
            Link link = null;
            if (linkId == null)
            {
                return link;
            }

            string sql = "select * from link where id=@link_id ";



            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("link_id", linkId);


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                link = CloneFromDataRow(row);

            }

            return link;


        }
        public List<Link> GetAllLink()
        {
            List<Link> linkList = new List<Link>();

            string sql = "select * from link order by sort_seq";


            IDbParameters dbParameters = CreateDbParameters();


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {

                Link link = CloneFromDataRow(row);
                linkList.Add(link);
            }

            return linkList;


        }

        public bool ExistsLink(string siteUrl)
        { 
            string querySql = "select count(1) from link where site_url=@site_url";
            IDbParameters queryDbParameters = CreateDbParameters();

            queryDbParameters.AddWithValue("site_url", siteUrl);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, querySql, queryDbParameters);
            if ((int)ds.Tables[0].Rows[0][0] > 0)
                return true;
            else
                return false;
        }
      
        public int InsertLink(Link link)
        {
            string sql = "insert into link (id,site_name,site_url,site_logo,description,my_url,approve_status,enabled,sort_seq,addtime,site_id,is_static,last_mod) values (@link_id,@site_name,@site_url,@site_logo,@description,@my_url,@approve_status,@enabled,@sort_seq,getdate(),@site_id,0,getdate())";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("link_id", link.link_id);
            dbParameters.AddWithValue("site_name", link.link_site_name);
            dbParameters.AddWithValue("site_url", link.link_site_url);
            dbParameters.AddWithValue("site_logo", link.link_site_logo);
            dbParameters.AddWithValue("description", link.link_description);
            dbParameters.AddWithValue("my_url", link.my_url);
            dbParameters.AddWithValue("approve_status", link.approve_status);
            dbParameters.AddWithValue("enabled", Convert.ToInt32(link.enabled));
            dbParameters.AddWithValue("sort_seq", (int)link.sort_seq);
            dbParameters.AddWithValue("site_id", Total.SiteId);
           


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int UpdateLink(Link link)
        {
            if (link == null || string.IsNullOrEmpty(link.link_id))
                return 0;


            string sql = "update link set site_name=@site_name,site_url=@site_url,site_logo=@site_logo,description=@description,my_url=@my_url,approve_status=@approve_status,approve_time=@approve_time,enabled=@enabled,addtime=@addtime,sort_seq=@sort_seq,last_mod=getdate(),is_static=0,site_id=@site_id where id=@link_id";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("link_id", link.link_id);
            dbParameters.AddWithValue("site_name", link.link_site_name);
            dbParameters.AddWithValue("site_url", link.link_site_url);
            dbParameters.AddWithValue("site_logo", link.link_site_logo);
            dbParameters.AddWithValue("description", link.link_description);
            dbParameters.AddWithValue("my_url", link.my_url);
            dbParameters.AddWithValue("approve_status", link.approve_status);
            dbParameters.AddWithValue("enabled", Convert.ToInt32(link.enabled));
            dbParameters.AddWithValue("addtime", link.addtime);
            dbParameters.AddWithValue("sort_seq", link.sort_seq);
            if (link.approve_time.Equals(DateTime.MinValue))
            {
                dbParameters.AddWithValue("approve_time", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("approve_time", link.approve_time);
            }
            dbParameters.AddWithValue("site_id", Total.SiteId); 

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int DeleteLink(string lid)
        {
            if (string.IsNullOrEmpty(lid))
                return 0;


            string sql = "delete from link where id=@link_id and site_id=@site_id";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("link_id", lid);
            dbParameters.AddWithValue("site_id", Total.SiteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
    }
}