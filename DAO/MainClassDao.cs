using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Data.Core;
using Spring.Data.Common;
namespace com.hujun64.Dao
{
    /// <summary>
    ///CommonDao 的摘要说明
    /// </summary>
    public class MainClassDao : AdoDaoSupport, IMainClassDao
    {

        public MainClass GetClassById(string classId)
        {

            MainClass mainClass = new MainClass();

            if (string.IsNullOrEmpty(classId))
                return mainClass;


            string sql = "select * from view_class where id=@classId";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("classId", classId);

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                mainClass = CloneMainClassFromDataRow(ds.Tables[0].Rows[0]);
            }
            return mainClass;



        }

        public int UpdateMainClass(MainClass mainClass)
        {

            string sql = "update view_class set id=@id,class_name=@class_name,class_parent=@class_parent,site_id=@site_id where id=@id";

            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("id", mainClass.id);
            dbParameters.AddWithValue("class_name", mainClass.class_name);
            dbParameters.AddWithValue("class_parent", mainClass.class_parent);
            dbParameters.AddWithValue("site_id", Total.SiteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }



        private MainClass CloneFromReader(DbDataReader reader)
        {
            MainClass mainClass = new MainClass();

            mainClass.id = Convert.ToString(reader["id"]);
            mainClass.class_name = Convert.ToString(reader["class_name"]);
            if (reader["class_parent"] != null)
                mainClass.class_parent = Convert.ToString(reader["class_parent"]);
            else
                mainClass.class_parent = "";

            mainClass.enabled = Convert.ToBoolean(reader["enabled"]);




            return mainClass;

        }

        private MainClass CloneMainClassFromDataRow(DataRow row)
        {
            MainClass mainClass = new MainClass();

            mainClass.id = Convert.ToString(row["id"]);
            mainClass.class_name = Convert.ToString(row["class_name"]);
            if (row["class_parent"] != null)
                mainClass.class_parent = Convert.ToString(row["class_parent"]);
            else
                mainClass.class_parent = "";


            mainClass.enabled = Convert.ToBoolean(row["enabled"]);



            return mainClass;

        }



        public DataSet GetBigClassDataSet(string siteId, bool withAll)
        {
            string sql = "select distinct id,class_name,checked='' from view_class where class_parent=0 and category='" + MainClassCategory.DropdownMenu + "'";
            if (!string.IsNullOrEmpty(siteId))
            {
                sql += " and site_id='" + siteId + "'";
            }




            DataSet dataSet = AdoTemplate.DataSetCreate(CommandType.Text, sql);


            DataRow rowOther = dataSet.Tables[0].NewRow();
            rowOther[0] = "";
            rowOther[1] = "其他";
            rowOther[2] = "checked";
            dataSet.Tables[0].Rows.Add(rowOther);

            if (withAll)
            {
                DataRow rowALl = dataSet.Tables[0].NewRow();
                rowALl[0] = "all";
                rowALl[1] = "所有";
                rowALl[2] = "checked";

                dataSet.Tables[0].Rows.Add(rowALl);
            }

            return dataSet;
        }
        public List<SiteClass> GetSiteClassList(string siteId)
        {
            List<SiteClass> siteClassList = new List<SiteClass>();

            StringBuilder sqlSb = new StringBuilder("select distinct sc.*,mc.* from site_class sc,main_class mc ");
            sqlSb.Append(" where sc.class_id=mc.id and mc.enabled=1 ");
            if (!string.IsNullOrEmpty(siteId))
                sqlSb.AppendFormat(" and site_id = '{0}'", siteId);
            sqlSb.Append(" order by sc.site_id,sc.sort_seq");


            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sqlSb.ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            { 
                siteClassList.Add(CloneSiteClassFromDataRow(row));
            }

            return siteClassList;

        }
        public List<MainClass> GetAllClassList()
        {

            List<MainClass> classList = new List<MainClass>();


            StringBuilder sqlSb = new StringBuilder();
            sqlSb.Append("select * from main_class where enabled=1  order by class_parent");

            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sqlSb.ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                MainClass mainClass = CloneMainClassFromDataRow(row);

                classList.Add(mainClass);
            }

            return classList;

        }
        public List<string> GetAllModuleViewIdList()
        {
            List<string> idList = new List<string>();

            string sql = "select id from view_module_class where site_id=@site_id union select id from view_module_index where site_id=@site_id union select id from view_module_leftmenu where site_id=@site_id ";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("site_id", Total.SiteId);
            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                idList.Add(row["id"].ToString());
            }

            return idList;

        }
        public int AddSiteClass(string siteId, string classId, string category)
        {
            string sql = "if not exists (select 1 from site_class where site_id=@site_id and class_id=@class_id) insert into site_class (site_id,class_id,category) values (@site_id,@class_id,@category)";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("site_id", siteId);
            dbParameters.AddWithValue("class_id", classId);
            dbParameters.AddWithValue("category", category);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }
        public int RemoveSiteClass(string siteId, string classId)
        {
            string sql = "delete from site_class where site_id=@site_id and class_id=@class_id";
            IDbParameters dbParameters = CreateDbParameters();


            dbParameters.AddWithValue("site_id", siteId);
            dbParameters.AddWithValue("class_id", classId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);

        }

        private SiteClass CloneSiteClassFromDataRow(DataRow row)
        {
            SiteClass siteClass = new SiteClass();

            siteClass.site_id = row["site_id"].ToString();
            siteClass.mainClass = this.CloneMainClassFromDataRow(row);
            siteClass.category = row["category"].ToString();
            siteClass.sort_seq = (int)row["sort_seq"];
            siteClass.template_url = Convert.ToString(row["template_url"]);
            if (row["img_url"] != null)
                siteClass.img_url = Convert.ToString(row["img_url"]);

            return siteClass;
        }
        public List<SiteClass> Get1stDMClassIdList(string siteId)
        {
            List<SiteClass> siteClassList = new List<SiteClass>();

            StringBuilder sqlSb = new StringBuilder("select * from view_1st_dm_class ");
            if (!string.IsNullOrEmpty(siteId))
                sqlSb.AppendFormat(" where site_id = '{0}'", siteId);
            sqlSb.Append(" order by site_id,sort_seq");


            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sqlSb.ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            { 
                siteClassList.Add(CloneSiteClassFromDataRow(row));
            }

            return siteClassList;
        }
        public List<SiteClass> Get2ndDMClassIdList(string siteId)
        {
            List<SiteClass> siteClassList = new List<SiteClass>();

            StringBuilder sqlSb = new StringBuilder("select * from view_2nd_dm_class");
            if (!string.IsNullOrEmpty(siteId))
                sqlSb.AppendFormat(" where site_id = '{0}'", siteId);
            sqlSb.Append(" order by site_id,sort_seq");


            DataSet ds = AdoTemplate.DataSetCreate(CommandType.Text, sqlSb.ToString());
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                siteClassList.Add(CloneSiteClassFromDataRow(row));
            }

            return siteClassList;
        }

    }
}