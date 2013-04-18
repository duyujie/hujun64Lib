using System.Collections.Generic;
 using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System;
using com.hujun64.po;
namespace com.hujun64.Dao
{
    /// <summary>
    ///ICommonDao 的摘要说明
    /// </summary>
    public interface ICommonDao 
    {

        DataSet GetDataSet(string sql);     
        DataView GetDataView(string sql);
        int ExecuteNonQuery(string sql);
        object ExecuteScalar(string sql);

       
        DateTime GetSysdate();

        int NewSeq(string class_name);
        int GetNextSeq(string class_name);
        List<GlobalSeq> GetAllGlobalSeq();
        int UpdateGlobalSeq(List<GlobalSeq> globalSeqList);

        List<Site> GetSiteList();

        


        List<GlobalConfig> GetAllGlobalConfig();
        GlobalConfig GetGlobalConfig(string configName, string siteId);
        int InsertGlobalConfig(GlobalConfig config);
        int UpdateGlobalConfig(GlobalConfig config);

        string GetSiteIdByName(string siteName);


      
        int PurgeLog(int dayExpires);
        int ImportIpLib(string ipFilename);
       
        int MaintainGlobalConfig(GlobalConfig globalConfig);

        DataSet GetPagerDataSet(string sql, int pageIndex, int pageSize, out int totalCount);



        List<CgwWords> GetCgwWordsList();
        int InsertCgwWords(CgwWords words);
        int DeleteCgwWords(string wordsId);
        int UpdateCgwWords(CgwWords words);

        bool? GetSiteStatus(string siteUrl);
        int InsertSiteStatus(string siteUrl, bool status);
        int UpdateSiteStatus(string siteUrl, bool status);



    }
}