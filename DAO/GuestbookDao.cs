using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using com.hujun64.po;
using com.hujun64.util;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    ///GuestbookDaoImpl 的摘要说明
    /// </summary>
    public class GuestbookDao : AdoDaoSupport, IGuestbookDao
    {
        private static readonly log4net.ILog mylog = log4net.LogManager.GetLogger("GuestbookDao");


        public Dictionary<string, string> GetAllGuestEmailDict(bool isOnlyShanghai)
        {
            Dictionary<string, string> guestEmailDict = new Dictionary<string, string>();

            string sql = "select distinct author,email from view_guestbook where email is not null and email <>'' and site_id=@site_id";
            if (isOnlyShanghai)
            {
                sql += " and province_from like '上海%'";
            }

            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                guestEmailDict.Add(row[0].ToString(), row[1].ToString());
            }

            return guestEmailDict;

        }


        public int CountGuestbook(string bigClassId)
        {

            StringBuilder sqlSb = new StringBuilder("select count(1) from view_guestbook where enabled=1  and site_id=@site_id");


            if (!string.IsNullOrEmpty(bigClassId))
                sqlSb.Append(" and big_class_id = @bigClassId ");




            IDbParameters dbParameters = CreateDbParameters();


            if (!string.IsNullOrEmpty(bigClassId))
                dbParameters.AddWithValue("bigClassId", bigClassId);



            dbParameters.AddWithValue("site_id", Total.SiteId);

            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sqlSb.ToString(), dbParameters);

        }
        public bool ExistsId(string guestbookId)
        {
            bool isExits = false;


            IDbParameters dbParameters = CreateDbParameters();


            string sql = "select count(1) from guestbook where id=@guestbookId";

            dbParameters.AddWithValue("guestbookId", guestbookId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsGuestbookSite(string guestbookId, string siteId)
        {
            bool isExits = false;


            IDbParameters dbParameters = CreateDbParameters();


            string sql = "select count(1) from site_guestbook where guestbook_id=@guestbookId and site_id=@siteId";

            dbParameters.AddWithValue("guestbookId", guestbookId);
            dbParameters.AddWithValue("siteId", siteId);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;
        }
        public bool ExistsTitle(string title)
        {
            bool isExits = false;


            IDbParameters dbParameters = CreateDbParameters();


            string sql = "select count(1) from guestbook where title=@title";

            dbParameters.AddWithValue("title", title);

            int count = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters));
            if (count > 0)
            {
                isExits = true;

            }

            return isExits;

        }
        public int UpdateGuestbook(Guestbook guestbook)
        {

            


            IDbParameters dbParameters = CreateDbParameters();

            string sql = "update guestbook set reply=@reply,big_class_id=@big_class_id,keywords=@keywords,addtime=@addtime,replytime=@replytime,title=@title,content=@content,is_static=@is_static,enabled=@enabled,last_mod=@last_mod where id=@id";


            if (guestbook.keywords == null)
                guestbook.keywords = "";

            dbParameters.AddWithValue("id", guestbook.id);

            dbParameters.AddWithValue("keywords", guestbook.keywords);
            if (string.IsNullOrEmpty(guestbook.reply))
            {
                dbParameters.AddWithValue("reply", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("reply", guestbook.reply);
            }
            if (guestbook.big_class_id == "" || guestbook.big_class_id == null)
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", guestbook.big_class_id);
            }
            dbParameters.AddWithValue("title", guestbook.title);
            dbParameters.AddWithValue("content", guestbook.content);
            dbParameters.AddWithValue("is_static", Convert.ToInt32(guestbook.is_static));
            dbParameters.AddWithValue("enabled", Convert.ToInt32(guestbook.enabled));

            if (DateTime.MinValue.Equals(guestbook.addtime))
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", guestbook.addtime);
            }
            if (DateTime.MinValue.Equals(guestbook.replytime))
            {
                dbParameters.AddWithValue("replytime", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("replytime", guestbook.replytime);
            }

            if (DateTime.MinValue.Equals(guestbook.last_mod))
            {
                dbParameters.AddWithValue("last_mod", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("last_mod", guestbook.last_mod);
            }


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);


        }
        public Dictionary<string, DateTime> GetAllGuestbookIdDict(bool isRefreshAll)
        {
            Dictionary<string, DateTime> guestbookIdDict = new Dictionary<string, DateTime>();


            IDbParameters dbParameters = CreateDbParameters();
            string sql;
            if (isRefreshAll)
            {
                sql = "select id,last_mod from view_guestbook where enabled=1 and site_id=@site_id";
            }
            else
            {
                sql = "select id,last_mod from view_guestbook where enabled=1 and is_static=0 and site_id=@site_id";
            }

            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                guestbookIdDict.Add(row[0].ToString(), Convert.ToDateTime(row[1]));
            }

            return guestbookIdDict;

        }
        public DataSet GetTopGeustbookDataSet(string bigClassId, int topNum)
        {

            IDbParameters dbParameters = CreateDbParameters();

            string sql = "select top " + topNum + " * from view_guestbook where enabled=1  and site_id=@site_id";
            if (!string.IsNullOrEmpty(bigClassId))
            {
                sql += " and big_class_id=@bigClassId ";
            }

            sql += " order by addtime desc,id desc";


            dbParameters.AddWithValue("site_id", Total.SiteId);

            if (!string.IsNullOrEmpty(bigClassId))
            {
                dbParameters.AddWithValue("bigClassId", bigClassId);
            }
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            return ds;


        }
        public int GetMaxGuestbookIdSeq()
        {
            string sql = "select max(convert(int,substring(id,3,len(id)))) from  guestbook";

            return (int)AdoTemplate.ExecuteScalar(CommandType.Text, sql);
        }
        public List<Guestbook> GetAllGuestbook(bool isWithContent)
        {
            string sql;
            if (isWithContent)
                sql = "select * from view_guestbook where site_id=@site_id";
            else
                sql = "select id,author,sex,contact,title,keywords,addtime,enabled,replytime,big_class_id,is_static,ip_from,last_visited_time,email,last_mod,province_from,click from view_guestbook where site_id=@site_id";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_id", Total.SiteId);


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            List<Guestbook> guestbookList = new List<Guestbook>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                guestbookList.Add(CloneFromDataRow(row, isWithContent));

            }



            return guestbookList;

        }
        public Guestbook GetGuestbook(string gid, bool isWithContent)
        {
            Guestbook guestbook = null;
            if (gid == null)
            {
                return guestbook;
            }

            string sql;
            if (isWithContent)
                sql = "select * from view_guestbook where id=@id and site_id=@site_id";
            else
                sql = "select id,author,sex,contact,title,keywords,addtime,enabled,replytime,big_class_id,is_static,ip_from,last_visited_time,email,last_mod,province_from,click from view_guestbook where id=@id and site_id=@site_id";



            IDbParameters dbParameters = CreateDbParameters();



            dbParameters.AddWithValue("site_id", Total.SiteId);
            dbParameters.AddWithValue("id", gid);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                guestbook = CloneFromDataRow(ds.Tables[0].Rows[0], isWithContent);

            }

            if (guestbook == null)
                guestbook = new Guestbook();

            return guestbook;


        }
        private Guestbook CloneFromReader(SqlDataReader reader, bool isWithContent)
        {
            Guestbook guestbook = new Guestbook();
            guestbook.id = reader["id"].ToString();
            guestbook.title = reader["title"].ToString();
            guestbook.author = reader["author"].ToString();
            guestbook.contact = reader["contact"].ToString();
            guestbook.email = reader["email"].ToString();
            guestbook.addtime = (DateTime)reader["addtime"];
            guestbook.last_mod = (DateTime)reader["last_mod"];
            if (isWithContent)
            {
                guestbook.content = reader["content"].ToString();
                guestbook.reply = reader["reply"].ToString();
            }
            if (reader["replytime"] != DBNull.Value)
                guestbook.replytime = (DateTime)reader["replytime"];
            if (reader["keywords"] != DBNull.Value)
                guestbook.keywords = reader["keywords"].ToString();
            guestbook.big_class_id = reader["big_class_id"].ToString();
            guestbook.sex = reader["sex"].ToString();
            guestbook.enabled = Convert.ToBoolean(reader["enabled"]);
            guestbook.ip_from = reader["ip_from"].ToString();
            guestbook.province_from = reader["province_from"].ToString();
            guestbook.click = Convert.ToInt32(reader["click"]);

            return guestbook;
        }
        private Guestbook CloneFromDataRow(DataRow row, bool isWithContent)
        {
            Guestbook guestbook = new Guestbook();
            guestbook.id = row["id"].ToString();
            guestbook.title = row["title"].ToString();
            guestbook.author = row["author"].ToString();
            guestbook.contact = row["contact"].ToString();
            guestbook.email = row["email"].ToString();
            guestbook.addtime = (DateTime)row["addtime"];
            guestbook.last_mod = (DateTime)row["last_mod"];
            if (isWithContent)
            {
                guestbook.content = row["content"].ToString();
                guestbook.reply = row["reply"].ToString();
            }
            if (row["replytime"] != DBNull.Value)
                guestbook.replytime = (DateTime)row["replytime"];
            if (row["keywords"] != DBNull.Value)
                guestbook.keywords = row["keywords"].ToString();
            guestbook.big_class_id = row["big_class_id"].ToString();
            guestbook.sex = row["sex"].ToString();
            guestbook.enabled = Convert.ToBoolean(row["enabled"]);
            guestbook.ip_from = row["ip_from"].ToString();
            guestbook.province_from = row["province_from"].ToString();
            guestbook.click = Convert.ToInt32(row["click"]);

            return guestbook;
        }
        public string GetGuestbookTitle(string gid)
        {
            if (gid == null)
            {
                return "";
            }

            string sql = "select title from view_guestbook where id=@id and site_id=@site_id";

            string title = "";

            IDbParameters dbParameters = CreateDbParameters();




            dbParameters.AddWithValue("id", gid);
            dbParameters.AddWithValue("site_id", Total.SiteId);


            object objResult = AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters);
            if (objResult != null && !Convert.IsDBNull(objResult))
            {
                title = Convert.ToString(objResult);
            }


            return title;
        }
        public int ReplyGuestbook(Guestbook guestbook)
        {


            IDbParameters dbParameters = CreateDbParameters();

            string sql = "update guestbook set reply=@reply,big_class_id=@big_class_id,keywords=@keywords,replytime=getdate(),title=@title,content=@content,is_static=0,last_mod=getdate() where id=@id";


            if (guestbook.keywords == null)
                guestbook.keywords = "";

            dbParameters.AddWithValue("id", guestbook.id);

            dbParameters.AddWithValue("keywords", guestbook.keywords);
            if (string.IsNullOrEmpty(guestbook.reply))
            {
                dbParameters.AddWithValue("reply", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("reply", guestbook.reply);
            }
            if (guestbook.big_class_id == "" || guestbook.big_class_id == null)
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", guestbook.big_class_id);
            }
            dbParameters.AddWithValue("title", guestbook.title);
            dbParameters.AddWithValue("content", guestbook.content);


            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public void IncreaseClick(string guestbookId)
        {

            if (string.IsNullOrEmpty(guestbookId))
                return;


            StringBuilder sqlSb = new StringBuilder();
            sqlSb.Append("update guestbook set click=click+1,last_visited_time=getdate() where id=@guestbookId;");

            IDbParameters dbParameters = CreateDbParameters();



            dbParameters.AddWithValue("guestbookId", guestbookId);

            AdoTemplate.ExecuteNonQuery(CommandType.Text, sqlSb.ToString(), dbParameters);


        }
        public int InsertGuestbook(Guestbook guestbook)
        {

            IDbParameters dbParameters = CreateDbParameters();

            string sql = "insert into guestbook (id,author,sex,contact,email,title,content,big_class_id,ip_from,addtime,reply,replytime,enabled,last_mod,click,is_static,last_visited_time) values (@id,@author,@sex,@contact,@email,@title,@content,@big_class_id,@ip_from,@addtime,@reply,@replytime,@enabled,@last_mod,0,0,getdate())";



            dbParameters.AddWithValue("id", guestbook.id);
            dbParameters.AddWithValue("author", guestbook.author);
            dbParameters.AddWithValue("sex", guestbook.sex);
            dbParameters.AddWithValue("title", guestbook.title);
            dbParameters.AddWithValue("content", guestbook.content);
            dbParameters.AddWithValue("contact", guestbook.contact);
            dbParameters.AddWithValue("email", guestbook.email);
            if (guestbook.big_class_id == "" || guestbook.big_class_id == null)
            {
                dbParameters.AddWithValue("big_class_id", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("big_class_id", guestbook.big_class_id);
            }
            dbParameters.AddWithValue("ip_from", guestbook.ip_from);

            if (string.IsNullOrEmpty(guestbook.reply))
            {
                dbParameters.AddWithValue("reply", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("reply", guestbook.reply);
            }



            dbParameters.AddWithValue("enabled", Convert.ToInt32(guestbook.enabled));

            if (DateTime.MinValue.Equals(guestbook.addtime))
            {
                dbParameters.AddWithValue("addtime", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("addtime", guestbook.addtime);
            }
            if (DateTime.MinValue.Equals(guestbook.replytime))
            {
                dbParameters.AddWithValue("replytime", DBNull.Value);
            }
            else
            {
                dbParameters.AddWithValue("replytime", guestbook.replytime);
            }
            if (DateTime.MinValue.Equals(guestbook.last_mod))
            {
                dbParameters.AddWithValue("last_mod", DateTime.Now);
            }
            else
            {
                dbParameters.AddWithValue("last_mod", guestbook.last_mod);
            }




            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int InsertGuestbookSite(string guestbookId, string siteId)
        {

            IDbParameters dbParameters = CreateDbParameters();



            string sql = "insert into site_guestbook (site_id,guestbook_id) values (@site_id,@guestbook_id)";

            dbParameters.AddWithValue("site_id", siteId);
            dbParameters.AddWithValue("guestbook_id", guestbookId);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int DeleteGuestbook(string gid)
        {

            IDbParameters dbParameters = CreateDbParameters();

            string sql = " delete from guestbook where id=@gid";



            dbParameters.AddWithValue("gid", gid);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        public int DeleteGuestbookSite(string guestbookId, string siteId)
        {
            IDbParameters dbParameters = CreateDbParameters();

            string sql = "delete from site_guestbook where guestbook_id=@guestbook_id and site_id=@site_id";


            dbParameters.AddWithValue("site_id", siteId);
            dbParameters.AddWithValue("guestbook_id", guestbookId);




            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }


        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {

            IDbParameters dbParameters = CreateDbParameters();
            string sql = "update guestbook set is_static=@isStatic,last_mod=@timestamp where is_static <> @isStatic and addtime<=@timestamp and isnull(replytime,'2000-01-01')<=@timestamp";



            dbParameters.AddWithValue("isStatic", Convert.ToInt32(isStatic));
            dbParameters.AddWithValue("timestamp", timestamp);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);



        }
        public List<string> UpdateListStatic(ICollection<string> idList, bool isStatic, DateTime timestamp)
        {
            List<string> updatedIdList = new List<string>(idList.Count);

            IDbParameters dbParameters = CreateDbParameters();
            string sql = "update guestbook set is_static=@isStatic,last_mod=@timestamp where is_static <> @isStatic and addtime<=@timestamp and isnull(replytime,'2000-01-01')<=@timestamp and id=@guestbookId";




            dbParameters.AddWithValue("isStatic", Convert.ToInt32(isStatic));
            dbParameters.AddWithValue("timestamp", timestamp);
            dbParameters.Add("guestbookId", SqlDbType.Char);
            foreach (string id in idList)
            {
                dbParameters.SetValue("guestbookId", id);
                if (AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters) > 0)
                {
                    updatedIdList.Add(id);
                }
            }


            return updatedIdList;

        }

    }
}