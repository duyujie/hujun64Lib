using System.Collections.Generic;
 using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using com.hujun64.po;
using com.hujun64.type;
namespace com.hujun64.Dao
{
    /// <summary>
    ///ICommonDao 的摘要说明
    /// </summary>
    public interface IMainClassDao 
    {
       
        List<MainClass> GetAllClassList();

        List<SiteClass> GetSiteClassList(string siteId);
        List<SiteClass> Get1stDMClassIdList(string siteId);
        List<SiteClass> Get2ndDMClassIdList(string siteId);

     
        List<string> GetAllModuleViewIdList();
        int AddSiteClass(string siteId, string classId, string category);
        int RemoveSiteClass(string siteId, string classId);    
       
     
        int UpdateMainClass(MainClass mainClass);
       
    }
}