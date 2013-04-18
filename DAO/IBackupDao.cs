using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using com.hujun64.po;
namespace com.hujun64.Dao
{
    /// <summary>
    ///IArticleDao 的摘要说明
    /// </summary>
    public interface IBackupDao
    {
        bool IsNeedBackup();
        List<BackupPool> GetAllBackupPool();
        List<BackupPool> GetBackupPool(string type);
        bool InsertBackupId(string id, string type, string todo, string siteId);
        int ClearBackupPool(string id, string siteId);
    }
}