using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using com.hujun64.Dao;
using com.hujun64.po;
using com.hujun64.type;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
namespace com.hujun64.logic
{
    internal class MainClassService : IMainClassService
    {
        private static Dictionary<string, MainClass> CachedMainClassDict = null;
        private static List<string> CachedSmallClassKeyList = null;
        private static List<string> CachedBigClassKeyList = null;
        private static List<string> CachedModuleClassKeyList = null;
        private static Dictionary<string, SiteClass> CachedSiteClassDict = null; //key = site_id + ';' + class_id
        private static List<SiteClass> Cached1stDMClassList = null;
        private static List<SiteClass> Cached2ndDMClassList = null;



        private static object privateStaticObject = new Object();



        private IMainClassDao classDao;
        private IArticleDao articleDao;
        private ICommonDao commonDao;

        public MainClassService(IMainClassDao classDao, IArticleDao articleDao, ICommonDao commonDao)
        {
            this.classDao = classDao;
            this.articleDao = articleDao;
            this.commonDao = commonDao;
            Init();
        }
        private void Init()
        {

            if (CachedMainClassDict == null)
            {
                if (string.IsNullOrEmpty(Total.SiteId))
                    Total.SiteId = commonDao.GetSiteIdByName(Total.SiteName);

                CacheAllClass();
            }

        }
        public void RefreshCachedClass()
        {
            // Synchronize access to the shared member.
            lock (privateStaticObject)
            {
                CacheAllClass();
            }
        }

        private void CacheAllClass()
        {

            List<MainClass> mainClassList = classDao.GetAllClassList();
            List<SiteClass> CachedSiteClassList = classDao.GetSiteClassList(null);

            if (CachedMainClassDict == null)
                CachedMainClassDict = new Dictionary<string, MainClass>(mainClassList.Count);
            else
                CachedMainClassDict.Clear();

            if (CachedSiteClassDict == null)
                CachedSiteClassDict = new Dictionary<string, SiteClass>(CachedSiteClassList.Count);
            else
                CachedSiteClassDict.Clear();

            if (CachedSmallClassKeyList == null)
                CachedSmallClassKeyList = new List<string>();
            else
                CachedSmallClassKeyList.Clear();

            if (CachedBigClassKeyList == null)
                CachedBigClassKeyList = new List<string>();
            else
                CachedBigClassKeyList.Clear();

            if (CachedModuleClassKeyList == null)
                CachedModuleClassKeyList = new List<string>();
            else
                CachedModuleClassKeyList.Clear();

            
            foreach (MainClass mainClass in mainClassList)
            {
                CachedMainClassDict.Add(mainClass.id, mainClass);
            }



            foreach (SiteClass siteClass in CachedSiteClassList)
            {

                siteClass.mainClass = CachedMainClassDict[siteClass.mainClass.id];
                CachedSiteClassDict.Add(this.GenerateSiteClassDictKey(siteClass.site_id, siteClass.mainClass.id), siteClass);



                //添加classId,siteId的关系

                MainClass mClass = CachedMainClassDict[siteClass.mainClass.id];
                if (!mClass.siteClassList.Contains(siteClass))
                {
                    mClass.siteClassList.Add(siteClass);
                }


                if (!string.IsNullOrEmpty(siteClass.mainClass.class_parent))
                {
                    CachedSmallClassKeyList.Add(siteClass.mainClass.id);
                    if (!CachedBigClassKeyList.Contains(siteClass.mainClass.class_parent))
                        CachedBigClassKeyList.Add(siteClass.mainClass.class_parent);
                }
                else if (siteClass.category == MainClassCategory.DropdownMenu && !Total.IndexModuleClassIncludeBigClassNameList.Contains(siteClass.mainClass.class_name))
                {
                    if (!CachedBigClassKeyList.Contains(siteClass.mainClass.id))
                        CachedBigClassKeyList.Add(siteClass.mainClass.id);
                }
            }

            Cached1stDMClassList = classDao.Get1stDMClassIdList(Total.SiteId);

            
            foreach (SiteClass sc in Cached1stDMClassList)
            {
                if (sc.template_url.Contains("big_id=") && !CachedBigClassKeyList.Contains(sc.mainClass.id))
                {
                    CachedBigClassKeyList.Add(sc.mainClass.id);
                }

            }

            Cached2ndDMClassList = classDao.Get2ndDMClassIdList(Total.SiteId);

            foreach (SiteClass sc in Cached2ndDMClassList)
            {
                if (sc.template_url.Contains("big_id=") && !CachedBigClassKeyList.Contains(sc.mainClass.id))
                {
                    CachedBigClassKeyList.Add(sc.mainClass.id);
                }

            }


        }
        public List<MainClass> GetClassList(List<string> classIdList)
        {
            List<MainClass> classList = new List<MainClass>();
            foreach (string id in classIdList)
            {
                classList.Add(CachedMainClassDict[id]);
            }
            return classList;
        }
        public List<MainClass> GetClassList(string siteId)
        {
            List<MainClass> classList = GetAllClassList();
            if (string.IsNullOrEmpty(siteId))
            {
                return classList;
            }
            else
            {
                List<MainClass> retClassList = new List<MainClass>(classList.Count);
                foreach (MainClass mainClass in classList)
                {
                    foreach (SiteClass sc in mainClass.siteClassList)
                    {
                        if (sc.site_id.Equals(siteId))
                        {
                            retClassList.Add(mainClass);
                        }
                    }
                }
                return retClassList;
            }
        }
        public MainClass GetCachedClassById(string id)
        {
            if (!string.IsNullOrEmpty(id) && CachedMainClassDict.ContainsKey(id))

                return CachedMainClassDict[id];
            else
                return new MainClass();


        }
        public MainClass GetCachedClassByName(string name, string siteId)
        {
            if (!string.IsNullOrEmpty(name))
            {
                foreach (SiteClass sc in CachedSiteClassDict.Values)
                {
                    if (sc.mainClass.class_name == name && sc.site_id.Equals(siteId))
                        return sc.mainClass;
                }
            }

            return new MainClass();


        }
        public MainClass GetCachedClassByPageType(PageType pageType)
        {
            switch (pageType)
            {
                case PageType.INDEX_TYPE:
                    return GetCachedClassByName(Total.ModuleNameIndex, Total.SiteId);
                case PageType.GUESTBOOK_TYPE:
                    return GetCachedClassByName(Total.ModuleNameFlzx, Total.SiteId);
                case PageType.INTRO_TYPE:
                    return GetCachedClassByName(Total.ModuleNameLsjs, Total.SiteId);
                case PageType.FEE_TYPE:
                    return GetCachedClassByName(Total.ModuleNameSfbz, Total.SiteId);
                case PageType.LINK_ALL_TYPE:
                    return GetCachedClassByName(Total.ModuleNameYqlj, Total.SiteId);
                default:
                    return new MainClass();
            }
        }
        public List<MainClass> GetAllClassList()
        {
            List<MainClass> classList = new List<MainClass>();
            classList.AddRange(CachedMainClassDict.Values);

            return classList;
        }

        public Hashtable GetSmallBigClassIdHash()
        {
            Hashtable hashSmallBig = new Hashtable();

            foreach (string smallClassKey in CachedSmallClassKeyList)
            {
                MainClass smallClass = GetCachedClassById(smallClassKey);

                hashSmallBig.Add(smallClass.id, smallClass.class_parent);
            }
            return hashSmallBig;

        }
        public MainClass GetClassById(string classId)
        {
            return GetCachedClassById(classId);

        }
        public List<MainClass> GetModuleClassListByModuleNameList(ICollection<string> moduleNameList, string siteId)
        {
            List<MainClass> classList = new List<MainClass>();
            foreach (SiteClass sc in CachedSiteClassDict.Values)
            {
                if (string.IsNullOrEmpty(siteId) || sc.site_id.Equals(siteId))
                {
                    if (moduleNameList.Contains(sc.mainClass.class_name) && this.GetClassByName(sc.mainClass.class_name, siteId).Equals(sc.mainClass))
                    {
                        classList.Add(sc.mainClass);
                    }
                }
            }
            return classList;

        }
        private string GenerateSiteClassDictKey(string siteId, string mainClassId)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(siteId);
            sb.Append(";");
            sb.AppendFormat(mainClassId);

            return sb.ToString();
        }


        public List<MainClass> GetModuleClassList(string siteId)
        {
            List<MainClass> classList = GetClassList(siteId);

            List<MainClass> bigClassList = this.GetBigClassList(siteId, false);
            foreach (MainClass bigClass in bigClassList)
            {
                classList.Remove(bigClass);

                List<MainClass> smallClassList = this.GetSmallClassList(bigClass.id, siteId);
                foreach (MainClass smallClass in smallClassList)
                {
                    if (classList.Contains(smallClass))
                        classList.Remove(smallClass);
                }
            }




            classList.Sort(new ComparerMainClass());
            return classList;

        }

        public string GetClassNameById(string classId)
        {

            return GetClassById(classId).class_name;

        }

        public SiteClass GetSiteClassByMain(MainClass mainClass, string siteId)
        {
            string key = GenerateSiteClassDictKey(siteId, mainClass.id);
            if (CachedSiteClassDict.ContainsKey(key))
                return CachedSiteClassDict[key];
            else
                return null;


        }
        public List<SiteClass> GetSiteClassByMain(List<MainClass> mainClassList, string siteId)
        {
            List<SiteClass> siteClassList = new List<SiteClass>(mainClassList.Count);
            foreach (MainClass mc in mainClassList)
            {
                if (mc.IsSiteReldated(siteId))
                {
                    foreach (SiteClass sc in mc.siteClassList)
                    {
                        if (sc.site_id.Equals(siteId))
                            siteClassList.Add(sc);
                    }
                }
            }
            return siteClassList;

        }
        public SiteClass GetSiteClassByName(string className, string siteId)
        {
            MainClass mainClass = this.GetClassByName(className, siteId);
            string key = GenerateSiteClassDictKey(siteId, mainClass.id);
            if (CachedSiteClassDict.ContainsKey(key))
                return CachedSiteClassDict[key];
            else
                return null;


        }
        public MainClass GetClassByName(string className, string siteId)
        {
            return GetCachedClassByName(className, siteId);


        }



        public MainClass GetBigClassBySmall(string smallClassId)
        {
            return GetClassById(GetClassById(smallClassId).class_parent);

        }
        public List<MainClass> GetSmallClassList(string bigClassId, string siteId)
        {
            List<MainClass> smallClassList = new List<MainClass>();
            if (!string.IsNullOrEmpty(bigClassId) && CachedBigClassKeyList.Contains(bigClassId))
            {
                foreach (string smallClassKey in CachedSmallClassKeyList)
                {
                    MainClass smallClass = GetCachedClassById(smallClassKey);
                    if (string.IsNullOrEmpty(siteId) || smallClass.IsSiteReldated(siteId))
                    {
                        if (smallClass.class_parent == bigClassId)
                            smallClassList.Add(smallClass);
                    }
                }
            }


            return smallClassList;

        }
        public List<MainClass> GetSmallClassByBig(string bigClassId)
        {
            return GetSmallClassList(bigClassId, null);


        }
        public string GetModuleUrl(string moduleName)
        {

            SiteClass siteClass = this.GetSiteClassByName(moduleName, Total.SiteId);
            if (siteClass != null && !string.IsNullOrEmpty(siteClass.mainClass.id))
                return siteClass.template_url;
            else
                return "";


        }
        public List<string> GetAllBigClassIdList()
        {

            return CachedBigClassKeyList;
        }

        public List<string> GetAllBigClassIdList(bool isRefreshAll)
        {

            if (isRefreshAll)
            {
                return GetAllBigClassIdList();
            }
            else
            {
                return articleDao.GetUnstaticBigClassIdList();

            }

        }

        public List<string> GetAllModuleClassIdList()
        {

            List<string> moduleList = CachedModuleClassKeyList;
            List<MainClass> moduleViewIdList = this.GetClassList(GetAllModuleViewIdList());
            foreach (MainClass moduleClass in moduleViewIdList)
            {

                if (!moduleList.Contains(moduleClass.id))
                    moduleList.Add(moduleClass.id);
            }
            return moduleList;
        }

        public List<string> GetAllModuleClassIdList(bool isRefreshAll)
        {
            List<string> allModuleClassIdList = new List<string>();
            if (isRefreshAll)
            {
                allModuleClassIdList.AddRange(GetAllModuleClassIdList());
            }
            else
            {
                allModuleClassIdList.AddRange(articleDao.GetUnstaticModuleClassIdList());

            }

            foreach (String name in Total.IndexModuleClassIncludeBigClassNameList)
            {
                MainClass mainClass = this.GetClassByName(name, Total.SiteId);
                if (mainClass != null && !String.IsNullOrEmpty(mainClass.id) && !allModuleClassIdList.Contains(mainClass.id))
                {
                    allModuleClassIdList.Add(mainClass.id);
                }

            }
            return allModuleClassIdList;

        }

        public List<string> GetAllSmallClassIdList()
        {
            return CachedSmallClassKeyList;
        }

        public List<string> GetAllSmallClassIdList(bool isRefreshAll)
        {
            if (isRefreshAll)
            {
                return GetAllSmallClassIdList();
            }
            else
            {
                return articleDao.GetUnstaticSmallClassIdList();

            }
        }


        public List<MainClass> GetClassByParent(string parentClassId, string siteId)
        {

            List<MainClass> mainClassList = new List<MainClass>();
            if (!string.IsNullOrEmpty(siteId) && CachedSiteClassDict.ContainsKey(siteId))
            {

                foreach (MainClass mainClass in CachedMainClassDict.Values)
                {
                    if (mainClass.IsSiteReldated(siteId) && parentClassId == mainClass.class_parent && !mainClassList.Contains(mainClass))
                    {
                        mainClassList.Add(mainClass);
                    }
                }

            }
            else
            {
                foreach (MainClass mainClass in CachedMainClassDict.Values)
                {
                    if (parentClassId == mainClass.class_parent && !mainClassList.Contains(mainClass))
                    {
                        mainClassList.Add(mainClass);
                    }
                }
            }
            mainClassList.Sort(new ComparerMainClass());
            return mainClassList;
        }
        public List<DropMenuClass> GetDropMenuClassByBoot(int boot, string siteId)
        {
            List<DropMenuClass> menuClassList = new List<DropMenuClass>();
            List<DropMenuClass> dropMenuClassList = GetDropMenuClass(siteId);
            foreach (DropMenuClass menuClass in dropMenuClassList)
            {
                if (menuClass.boot == boot && menuClass.site_id.Equals(siteId))
                    menuClassList.Add(menuClass);
            }
            return menuClassList;
        }
        public List<DropMenuClass> GetDropMenuClassByParentBoot(string parentClassId, int boot, string siteId)
        {
            List<DropMenuClass> menuClassList = new List<DropMenuClass>();
            List<DropMenuClass> dropMenuClassList = GetDropMenuClass(siteId);
            foreach (DropMenuClass menuClass in dropMenuClassList)
            {
                if (menuClass.boot == boot && menuClass.mainClass.class_parent == parentClassId)
                    menuClassList.Add(menuClass);
            }
            return menuClassList;
        }
        public List<MainClass> GetTopMenuClass(string siteId)
        {


            List<MainClass> topMenuMainClassList = new List<MainClass>();
            if (string.IsNullOrEmpty(siteId))
            {
                return topMenuMainClassList;
            }

            List<SiteClass> topMenuSiteClassList = new List<SiteClass>();
            foreach (SiteClass siteClass in CachedSiteClassDict.Values)
            {
                if (siteClass.site_id == siteId && siteClass.mainClass.enabled && siteClass.category == MainClassCategory.TopMenu)
                {


                    topMenuSiteClassList.Add(siteClass);
                }
            }



            topMenuSiteClassList.Sort(new ComparerSiteClass());
            foreach (SiteClass menuClass in topMenuSiteClassList)
            {
                topMenuMainClassList.Add(menuClass.mainClass);
            }
            return topMenuMainClassList;
        }
        public List<DropMenuClass> Get1stDropMenuClass(string siteId)
        {
            List<DropMenuClass> dmList = new List<DropMenuClass>();
            foreach (SiteClass sc in Cached1stDMClassList)
            {
                dmList.Add(ConvertDropdownMenuClassFromSiteClass(sc));
            }
            return dmList;
        }
        public List<DropMenuClass> Get2ndDropMenuClass(string siteId)
        {
            List<DropMenuClass> dmList = new List<DropMenuClass>();
            foreach (SiteClass sc in Cached2ndDMClassList)
            {
                dmList.Add(ConvertDropdownMenuClassFromSiteClass(sc));
            }
            return dmList;
        }
        public List<DropMenuClass> GetDropMenuClass(string siteId)
        {
            List<DropMenuClass> dropMenuClassList = new List<DropMenuClass>();
            if (string.IsNullOrEmpty(siteId))
                return dropMenuClassList;


            foreach (MainClass mainClass in CachedMainClassDict.Values)
            {
                if (mainClass.IsSiteReldated(siteId) && mainClass.enabled)
                {
                    SiteClass siteClass = CachedSiteClassDict[this.GenerateSiteClassDictKey(siteId, mainClass.id)];
                    if (siteClass.category == MainClassCategory.DropdownMenu)
                    {
                        dropMenuClassList.Add(ConvertDropdownMenuClassFromSiteClass(siteClass));
                    }
                }

            }


            dropMenuClassList.Sort(new ComparerDropMenuClass());
            return dropMenuClassList;
        }
        private DropMenuClass ConvertDropdownMenuClassFromSiteClass(SiteClass siteClass)
        {
            DropMenuClass menuClass = new DropMenuClass();
            menuClass.Clone(siteClass);
            menuClass.sort_seq = siteClass.sort_seq;
            if (string.IsNullOrEmpty(menuClass.mainClass.class_parent))
                menuClass.boot = 0;
            else
                menuClass.boot = 1;

            return menuClass;
        }
        public DropMenuClass GetDropdownMenuClassByMainClass(MainClass mainClass, String siteId)
        {
            if (mainClass.IsSiteReldated(siteId) && mainClass.enabled)
            {
                SiteClass siteClass = CachedSiteClassDict[this.GenerateSiteClassDictKey(siteId, mainClass.id)];

                return ConvertDropdownMenuClassFromSiteClass(siteClass);
            }
            else
                return null;
        }



        [Transaction(TransactionPropagation.Required)]
        public int UpdateMainClass(MainClass mainClass)
        {
            return classDao.UpdateMainClass(mainClass);
        }

        public List<string> GetAllModuleViewIdList()
        {
            return classDao.GetAllModuleViewIdList();
        }

        public List<MainClass> GetBigClassList(string siteId, bool withAll)
        {
            List<MainClass> mainClassList;
            if (string.IsNullOrEmpty(siteId))
            {
                mainClassList = GetClassList(CachedBigClassKeyList);
            }
            else
            {
                mainClassList = new List<MainClass>();

                foreach (string bigClassId in CachedBigClassKeyList)
                {
                    MainClass bigClass = CachedMainClassDict[bigClassId];
                    if (bigClass.IsSiteReldated(siteId))
                    {
                        if (CachedSiteClassDict[this.GenerateSiteClassDictKey(siteId, bigClassId)].category == MainClassCategory.DropdownMenu)
                            mainClassList.Add(bigClass);
                    }
                }

            }

            MainClass allOtherMainClass = new MainClass();
            allOtherMainClass.id = "";
            if (withAll)
            {

                allOtherMainClass.class_name = "所有";

            }
            else
            {
                allOtherMainClass.class_name = "其他";

            }
            mainClassList.Add(allOtherMainClass);

            return mainClassList;

        }
        public string GetRadioBigClassNameById(string bigClassId)
        {
            string name = GetClassNameById(bigClassId);
            if (string.IsNullOrEmpty(name))
                name = "其他案件";
            else if (name.ToLower().Equals("all"))
                name = "所有";

            return name;
        }
        public List<RadioMainClass> GetRadioBigClassList(string siteId, bool withAll)
        {
            List<RadioMainClass> radioMainClassList = new List<RadioMainClass>();
            List<DropMenuClass> bigClassList = this.Get1stDropMenuClass(siteId);



            foreach (DropMenuClass bigClass in bigClassList)
            {
                
                    RadioMainClass radioBigClass = new RadioMainClass();
                    radioBigClass.Clone(bigClass.mainClass);
                    radioMainClassList.Add(radioBigClass);
                
            }
            RadioMainClass otherRadioMainClass = new RadioMainClass();
            otherRadioMainClass.id = "";
            otherRadioMainClass.class_name = "其他";
            otherRadioMainClass.radio_checked = true;
            radioMainClassList.Add(otherRadioMainClass);
            if (withAll)
            {
                RadioMainClass allRadioMainClass = new RadioMainClass();
                allRadioMainClass.id = "all";
                allRadioMainClass.class_name = "所有";
                allRadioMainClass.radio_checked = true;
                radioMainClassList.Add(allRadioMainClass);


            }
            return radioMainClassList;
        }
        [Transaction(TransactionPropagation.Required)]
        public int AddSiteClass(string siteId, string classId, string category)
        {
            int i = classDao.AddSiteClass(siteId, classId, category);
            this.RefreshCachedClass();
            return i;
        }
        [Transaction(TransactionPropagation.Required)]
        public int RemoveSiteClass(string siteId, string classId)
        {
            int i = classDao.RemoveSiteClass(siteId, classId);
            this.RefreshCachedClass();
            return i;
        }

        public List<SiteClass> CleanTemplateUrlOfSiteClass(List<SiteClass> classList)
        {
            foreach (SiteClass category in classList)
            {
                int i = category.template_url.IndexOf("?");
                if (i > 0)
                    category.template_url = category.template_url.Substring(0, i);
            }
            return classList;
        }
    }
}
