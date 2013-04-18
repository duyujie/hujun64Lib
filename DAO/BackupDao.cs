using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;
using com.hujun64.po;
using com.hujun64.util;
using Spring.Data.Core;
using Spring.Data.Common;

namespace com.hujun64.Dao
{
    /// <summary>
    ///BackupDao  的摘要说明
    /// </summary>
    public class BackupDao : AdoDaoSupport, IBackupDao
    {

        public bool InsertBackupId(string id, string type, string todo,string siteId)
        {
            try
            {
                
                string sql = "if not exists (select 1 from backup_pool where id=@id and site_id=@site_id) insert into backup_pool (id,type,todo,site_id) values (@id,@type,@todo,@site_id) ";
                IDbParameters dbParameters = CreateDbParameters();
                dbParameters.AddWithValue("id", id);
                dbParameters.AddWithValue("type", type);
                dbParameters.AddWithValue("todo", todo);
                dbParameters.AddWithValue("site_id",siteId);

                int i = AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
                if (i > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                log.Error("Insert backup_pool error!", ex);
                return false;
            }

        }
        public bool IsNeedBackup()
        {
            bool ret = false;

            log.Info("Check IsNeedBackup");


            StringBuilder sqlSb = new StringBuilder("select count(1) from backup_pool");


            IDbParameters dbParameters = CreateDbParameters();

            if ((int)AdoTemplate.ExecuteScalar(CommandType.Text, sqlSb.ToString(), dbParameters) > 0)
            {
                ret = true;

            }


            return ret;

        }
        private BackupPool CloneFromDataRow(DataRow row)
        {
            BackupPool backupPool = new BackupPool();
            backupPool.id = Convert.ToString(row["id"]);
            backupPool.todo = Convert.ToString(row["todo"]);
            backupPool.type = Convert.ToString(row["type"]);
            backupPool.site_id = Convert.ToString(row["site_id"]);

            return backupPool;

        }
        public List<BackupPool> GetBackupPool(string type)
        {

            string sql = "select id,todo,type,site_id from backup_pool where type=@type";


            IDbParameters dbParameters = CreateDbParameters();

            dbParameters.AddWithValue("type", type);

            List<BackupPool> backupPoolList = new List<BackupPool>();

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                backupPoolList.Add(CloneFromDataRow(row));
            }


            return backupPoolList;


        }
        public List<BackupPool> GetAllBackupPool()
        {

            string sql = "select id,todo,type,site_id from backup_pool";


            IDbParameters dbParameters = CreateDbParameters();


            List<BackupPool> backupPoolList = new List<BackupPool>();

            DataSet ds = AdoTemplate.DataSetCreateWithParams(CommandType.Text, sql, dbParameters);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                backupPoolList.Add(CloneFromDataRow(row));
            }


            return backupPoolList;


        }

        public int ClearBackupPool(string id,string siteId)
        {

            string sql = "delete from backup_pool where id=@id and site_id=@site_id";
            IDbParameters dbParameters = CreateDbParameters();
            dbParameters.AddWithValue("id", id);
            dbParameters.AddWithValue("site_id", siteId);

            return AdoTemplate.ExecuteNonQuery(CommandType.Text, sql, dbParameters);
        }
    }

}