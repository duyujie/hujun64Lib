using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.hujun64.po;
using com.hujun64.type;
namespace com.hujun64.logic
{
    public interface IMainClassService
    {

        List<MainClass> GetAllClassList();
        List<MainClass> GetBigClassList(string siteId, bool withAll);
        List<RadioMainClass> GetRadioBigClassList(string siteId, bool withAll);
        string GetRadioBigClassNameById(string bigClassId);
        List<MainClass> GetSmallClassList(string siteId, string bigClassId);
        List<MainClass> GetSmallClassByBig(string bigClassId);

        List<MainClass> GetModuleClassList(string siteId);
        List<MainClass> GetModuleClassListByModuleNameList(ICollection<string> moduleNameList, string siteId);
        List<MainClass> GetClassByParent(string parentClassId, string siteId);

        List<MainClass> GetTopMenuClass(string siteId);
        DropMenuClass GetDropdownMenuClassByMainClass(MainClass mainClass, String siteId);

        List<DropMenuClass> GetDropMenuClass(string siteId);
        List<DropMenuClass> GetDropMenuClassByBoot(int boot, string siteId);
        List<DropMenuClass> GetDropMenuClassByParentBoot(string parentClassId, int boot, string siteId);


        MainClass GetCachedClassById(string id);
        MainClass GetCachedClassByName(string name, string siteId);
        MainClass GetCachedClassByPageType(PageType pageType);
        void RefreshCachedClass();

        List<SiteClass> GetSiteClassByMain(List<MainClass> mainClassList, string siteId);
        SiteClass GetSiteClassByMain(MainClass mainClass, string siteId);
        SiteClass GetSiteClassByName(string className, string siteId);
        List<MainClass> GetClassList(List<string> classIdList);
        MainClass GetBigClassBySmall(string smallClassId);
        MainClass GetClassByName(string className, string siteId);
        MainClass GetClassById(string classId);
        Hashtable GetSmallBigClassIdHash();
        string GetModuleUrl(string moduleName);
        List<string> GetAllModuleViewIdList();
        List<string> GetAllBigClassIdList(bool isRefreshAll);
        List<string> GetAllModuleClassIdList(bool isRefreshAll);
        List<string> GetAllSmallClassIdList(bool isRefreshAll);
        List<DropMenuClass> Get1stDropMenuClass(string siteId);
        List<DropMenuClass> Get2ndDropMenuClass(string siteId);

        int AddSiteClass(string siteId, string classId, string category);
        int RemoveSiteClass(string siteId, string classId);



        int UpdateMainClass(MainClass mainClass);


        List<SiteClass> CleanTemplateUrlOfSiteClass(List<SiteClass> classList);
    }
}
