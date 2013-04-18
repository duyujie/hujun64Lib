using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Data.Core;
using Spring.Data.Common;

namespace com.hujun64.Dao
{
    /// <summary>
    ///CommonDao 的摘要说明
    /// </summary>
    public class CommonDao : AdoDaoSupport, ICommonDao
    {
        public DataSet GetDataSet(string sql)
        {
            return AdoTemplate.DataSetCreate(CommandType.Text, sql);
        }
        public DataView GetDataView(string sql)
        {
            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);
            return new DataView(ds.Tables[0]);
        }
        public int ExecuteNonQuery(string sql)
        {
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql);
        }
        public object ExecuteScalar(string sql)
        {
            return AdoTemplate.ExecuteScalar(CommandType.Text, sql);
        }


        public DateTime GetSysdate()
        {
            string sql = "select getdate()";
            return Convert.ToDateTime(AdoTemplate.ExecuteScalar(CommandType.Text, sql));


        }
        public int NewSeq(string class_name)
        {
            int initSeq = 1;
            string sql=" insert global_seq (class_name, seq) values (@class_name,@seq)";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("class_name", class_name);
            dbParameters.AddWithValue("seq", initSeq);
            AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
            return initSeq;
        }
        public int GetNextSeq(string class_name)
        {
            StringBuilder sqlSb=new StringBuilder();
            sqlSb.Append(" update global_seq set seq=seq+1 where class_name=@class_name ");
            sqlSb.Append(" select seq from global_seq where class_name=@class_name");
            IDbParameters dbParameters = CreateDbParameters();        
            dbParameters.AddWithValue("class_name", class_name);
            DataSet ds=AdoTemplate.DataSetCreateWithParams(CommandType.Text, sqlSb.ToString(), dbParameters);
            if (ds.Tables[0].Rows.Count == 0)
                return NewSeq(class_name);
            else
                return (int)ds.Tables[0].Rows[0][0];

        }
        public GlobalConfig GetGlobalConfig(string configName,string siteId)
        {
            string sql = "select * from global_config where config_name=@config_name and site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("config_name", configName);
            dbParameters.AddWithValue("site_id", siteId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                return CloneGlobalConfigFromDataRow(row);
            }


            return new GlobalConfig();
        }

        public int InsertGlobalConfig(GlobalConfig config)
        {
            string sql = "insert into global_config (config_name,config_value,site_id) values (@config_name,@config_value,@site_id)";

            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("config_name", config.config_name);
            dbParameters.AddWithValue("config_value", config.config_value);
            dbParameters.AddWithValue("site_id", config.site_id);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int UpdateGlobalConfig(GlobalConfig config)
        {
            string sql = "update global_config set config_value=@config_value where config_name=@config_name and site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("config_name", config.config_name);
            dbParameters.AddWithValue("config_value", config.config_value);
            dbParameters.AddWithValue("site_id", config.site_id);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        private GlobalConfig CloneGlobalConfigFromDataRow(DataRow row)
        {
            GlobalConfig globalConfig = new GlobalConfig();
            globalConfig.config_name = Convert.ToString(row["config_name"]);
            globalConfig.config_value = Convert.ToString(row["config_value"]);
            globalConfig.site_id = Convert.ToString(row["site_id"]);

            return globalConfig;
        }

        public List<GlobalConfig> GetAllGlobalConfig()
        {



            string sql = "select * from global_config where  site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();



            dbParameters.AddWithValue("site_id", Total.SiteId);


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            List<GlobalConfig> globalConfigList = new List<GlobalConfig>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                globalConfigList.Add(CloneGlobalConfigFromDataRow(row));
            }


            return globalConfigList;

        }

        public List<GlobalSeq> GetAllGlobalSeq()
        {



            string sql = "select * from global_seq";


            IDbParameters dbParameters = CreateDbParameters();
           


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);

            List<GlobalSeq> globalSeqList = new List<GlobalSeq>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                GlobalSeq globalSeq = new GlobalSeq();
                globalSeq.class_name = Convert.ToString(row["class_name"]);
                globalSeq.seq = Convert.ToInt32(row["seq"]);            
                globalSeqList.Add(globalSeq);
            }
            return globalSeqList;
        }
        public int UpdateGlobalSeq(List<GlobalSeq> globalSeqList)
        {
            string sql = "update global_seq set seq=@seq where class_name=@class_name";
            IDbParameters dbParameters = CreateDbParameters();
            
            dbParameters.Add("seq", SqlDbType.Int);
            dbParameters.Add("class_name", SqlDbType.VarChar);
           

            int rowUpdated = 0;
            foreach (GlobalSeq globalSeq in globalSeqList)
            {
                dbParameters.SetValue("class_name",globalSeq.class_name);
                dbParameters.SetValue("seq",globalSeq.seq);
            

                rowUpdated += AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
            }
            return rowUpdated;



        }
        public int MaintainGlobalConfig(GlobalConfig globalConfig)
        {

            string sql = "update global_config set config_value=@config_value where config_name=@config_name and site_id=@site_id";

            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("config_name", globalConfig.config_name);
            dbParameters.AddWithValue("config_value", globalConfig.config_value);
            dbParameters.AddWithValue("site_id", Total.SiteId);


            int i = AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

            if (i <= 0)
            {
                sql = "insert into global_config (config_name,config_value,site_id) values (@config_name,@config_value,@site_id)";
                i = AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
            }

            return i;

        }


        public int ImportIpLib(string ipFilename)
        {
            StreamReader sr = new StreamReader(ipFilename);
            string ipLine = null;

            string sql = "insert into ip_lib (ip_from,ip_to,state,detail) values (@ip_from,@ip_to,@state,@detail)";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.Add("ip_from", SqlDbType.VarChar);
            dbParameters.Add("ip_to", SqlDbType.VarChar);
            dbParameters.Add("state", SqlDbType.NVarChar);
            dbParameters.Add("detail", SqlDbType.NVarChar);

            int i = 0;
            int index1, index2, index3;
            string ipFrom, ipTo, state, detail;

            while ((ipLine = sr.ReadLine()) != null)
            {
                if (ipLine == "")
                    break;

                //ip_from
                index1 = ipLine.IndexOf(" ");
                index2 = ipLine.Substring(index1 + 1).IndexOf(" ");
                index3 = ipLine.Substring(index1 + index2 + 2).IndexOf(" ");


                ipFrom = ipLine.Substring(0, index1);
                ipTo = ipLine.Substring(index1 + 1, index2);
                if (index3 > 0)
                {
                    state = ipLine.Substring(index1 + index2 + 2, index3);
                    detail = ipLine.Substring(index1 + index2 + index3 + 3);
                }
                else
                {
                    state = ipLine.Substring(index1 + index2 + 2);
                    detail = "";
                }

                dbParameters.SetValue("ip_from",ipFrom);
                dbParameters.SetValue("ip_to", ipTo);
               dbParameters.SetValue("state", state);
               dbParameters.SetValue("detail", detail);

                i += AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
            }

            return i;


        }
        public int PurgeLog(int dayExpires)
        {
            if (dayExpires < 1)
                dayExpires = 1;

            string sql = "delete from [Log] where [Date] <= getdate() - " + dayExpires;
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql);

        }


        public List<Site> GetSiteList()
        {
            List<Site> siteList = new List<Site>();

            string sql = "select site_id,site_name from site_info";
            IDbParameters dbParameters = CreateDbParameters();


            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Site site = new Site();
                site.site_id = row["site_id"].ToString();
                site.site_name = row["site_name"].ToString();
                siteList.Add(site);


            }
            return siteList;

        }


        public string GetSiteIdByName(string siteName)
        {


            string siteId = "1";
            string sql = "select site_id from site_info where site_name=@siteName";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("siteName", siteName.Trim());

            object objResult = AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters);

            if (objResult != null && !Convert.IsDBNull(objResult))
            {

                siteId = Convert.ToString(objResult);

            }
            return siteId;


        }

        public DataSet GetPagerDataSet(string sql, int pageIndex, int pageSize, out int totalCount)
        {
            if (pageIndex <= 0)
                pageIndex = 1;

            string countSql = UtilString.ConvertToCountSql(sql);
            totalCount = Convert.ToInt32(AdoTemplate.ExecuteScalar(CommandType.Text, countSql));

            SqlDataAdapter sda = new SqlDataAdapter(sql, AdoTemplate.DbProvider.ConnectionString);
            DataSet ds = new DataSet();

            sda.Fill(ds, pageSize * (pageIndex - 1), pageSize, "pagerTable");

            return ds;

        }
        public List<CgwWords> GetCgwWordsList()
        {
            List<CgwWords> wordsList = new List<CgwWords>();


            string sql = "select * from cgw";

            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sql);


            foreach (DataRow row in ds.Tables[0].Rows)
            {

                CgwWords words = new CgwWords();
                words.id = row["id"].ToString();
                words.original_words = row["original_words"].ToString();
                words.new_words = row["new_words"].ToString();

                wordsList.Add(words);
            }


            return wordsList;
        }
        public int InsertCgwWords(CgwWords words)
        {
            string sql = " insert into cgw (id,original_words,new_words) values (@id,@original_words,@new_words)";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", words.id);
            dbParameters.AddWithValue("original_words", words.original_words);
            dbParameters.AddWithValue("new_words", words.new_words);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int UpdateCgwWords(CgwWords words)
        {
            string sql = " update cgw set new_words=@new_words where original_words=@original_words";


            IDbParameters dbParameters = CreateDbParameters();

           
            dbParameters.AddWithValue("original_words", words.original_words);
            dbParameters.AddWithValue("new_words", words.new_words);



            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
        public int DeleteCgwWords(string originalWords)
        {
            string sql = " delete from cgw where original_words=@original_words";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("original_words", originalWords);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }

        public bool? GetSiteStatus(string siteUrl)
        {

            bool? status = null;
            string sql = "select status from site_monitor where site_url=@site_url";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_url", siteUrl);
            object objResult = AdoTemplate.ExecuteScalar(CommandType.Text, sql, dbParameters);
            if (objResult != null && !Convert.IsDBNull(objResult))
            {
                status = Convert.ToBoolean(objResult);

            }
            return status;

        }
        public int UpdateSiteStatus(string siteUrl, bool status)
        {
            string sql = "update site_monitor set status=@status,update_datetime=getdate() where site_url=@site_url";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_url", siteUrl);
            dbParameters.AddWithValue("status", status);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int InsertSiteStatus(string siteUrl, bool status)
        {

            string sql = "insert into site_monitor (site_url,status,update_datetime) values (@site_url,@status,getdate())";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("site_url", siteUrl);
            dbParameters.AddWithValue("status", status);
            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

    }
}