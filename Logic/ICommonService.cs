using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.hujun64.po;
using com.hujun64.type;

namespace com.hujun64.logic
{
    public interface ICommonService
    {
        /// <summary>
        /// 检查网站状态
        /// </summary>
        /// <param name="siteUrl">网站地址</param>
        /// <param name="status">状态</param>
        /// <returns>返回false表示状态已经改变，true表示状态未改变</returns>
        bool CheckSiteStatus(string siteUrl, bool status);

        bool CheckSiteStatus(string siteUrl);

        DataSet GetDataSet(string sql);
        DataView GetDataView(string sql);
        int ExecuteNonQuery(string sql);
        object ExecuteScalar(string sql);


        DateTime GetSysdate();

        int GetNextSeq(string class_name);
        List<GlobalSeq> GetAllGlobalSeq();
        string GetGlobalConfigValue(string configName);
        void SetGlobalConfigValue(string configName, string configValue);
       
        
        
        

        List<Site> GetSiteList();
        string GetSiteIdByName(string siteName);
        int PurgeLog(int dayExpires);
        int ImportIpLib(string ipFilename);
        int MaintainGlobalConfig(GlobalConfig globalConfig);
        int UpdateGlobalSeq(List<GlobalSeq> globalSeqList);

        DataSet GetPagerDataSet(string sql, int pageIndex, int pageSize, out int totalCount);

    }
}
