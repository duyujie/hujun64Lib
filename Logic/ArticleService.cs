using System;
using System.Collections.Generic;
using System.Text;
using com.hujun64.po;
using com.hujun64.Dao;
using com.hujun64.util;
using Spring.Transaction;
using Spring.Transaction.Interceptor;


namespace com.hujun64.logic
{
    internal class ArticleService : IArticleService
    {
        private static readonly object Locker = new object();

        private static Dictionary<string, Article> CachedArticleDict = null;
        private static Dictionary<string, string> CachedTitleKeyDict = null;
        private static Dictionary<string, List<string>> CachedClassArticleKeyDict = null;
        private static Dictionary<string, string> CachedRefArticleKeyDict = null;
        private static Dictionary<string, List<string>> CachedArticleSiteKeyDict = null;

        private static bool IS_CACHE_CONTENT = Total.IsCacheArticleContent;



        private IArticleDao articleDao;
        private ICommonDao commonDao;
        private IBackupDao backupDao;

        private static object privateStaticObject = new Object();


        public ArticleService(IArticleDao articleDao, ICommonDao commonDao, IBackupDao backupDao)
        {
            this.articleDao = articleDao;
            this.commonDao = commonDao;
            this.backupDao = backupDao;

            Init();
        }

        private void Init()
        {

            if (CachedArticleDict == null)
            {
                if (string.IsNullOrEmpty(Total.SiteId))
                    Total.SiteId = commonDao.GetSiteIdByName(Total.SiteName);

                CacheArticle();
            }


        }

        public void RefreshCachedArticle()
        {
            // Synchronize access to the shared member.
            lock (privateStaticObject)
            {
                CacheArticle();

            }
        }
        private void UpdateCachedArticle(List<Article> articleList)
        {
            foreach (Article article in articleList)
            {
                UpdateCachedArticle(article);
            }
        }
        private void UpdateCachedArticle(Article article)
        {
            if (string.IsNullOrEmpty(article.id))
            {
                //被purge
                RemoveCachedArticle(article.id);
            }
            else if (!CachedArticleDict.ContainsKey(article.id))
            {
                //新增
                NewCachedArticle(article);

            }
            else
            {
                //更新
                RemoveCachedArticle(article.id);
                NewCachedArticle(article);
            }
        }

        private void UpdateCachedArticle(string articleId)
        {
            Article article = articleDao.GetArticle(articleId, IS_CACHE_CONTENT);

            UpdateCachedArticle(article);
            if (!string.IsNullOrEmpty(article.ref_id))
                this.MaintainCachedArticleRef();
        }

        private void CacheArticle()
        {

            List<Article> articleList = articleDao.GetAllArticle(IS_CACHE_CONTENT);
            lock (Locker)
            {
                if (CachedArticleDict == null)
                    CachedArticleDict = new Dictionary<string, Article>(articleList.Count);
                else
                    CachedArticleDict.Clear();


                if (CachedTitleKeyDict == null)
                    CachedTitleKeyDict = new Dictionary<string, string>(articleList.Count);
                else
                    CachedTitleKeyDict.Clear();

                if (CachedClassArticleKeyDict == null)
                    CachedClassArticleKeyDict = new Dictionary<string, List<string>>();
                else
                    CachedClassArticleKeyDict.Clear();

                if (CachedRefArticleKeyDict == null)
                    CachedRefArticleKeyDict = new Dictionary<string, string>();
                else
                    CachedRefArticleKeyDict.Clear();


                foreach (Article article in articleList)
                {
                    NewCachedArticle(article);
                }

            }

            MaintainCachedArticleRef();

            MaintainCachedArticleSite();
        }
        private void RemoveCachedArticle(string articleId)
        {
            if (string.IsNullOrEmpty(articleId))
                return;
            lock (Locker)
            {

                if (CachedRefArticleKeyDict.ContainsKey(articleId))
                {
                    CachedRefArticleKeyDict.Remove(articleId);
                }
                if (CachedRefArticleKeyDict.ContainsValue(articleId))
                {
                    List<string> removeKeyList = new List<string>();
                    foreach (string key in CachedRefArticleKeyDict.Keys)
                    {
                        if (CachedRefArticleKeyDict[key] == articleId)
                            removeKeyList.Add(key);
                    }
                    foreach (string key in removeKeyList)
                    {
                        CachedRefArticleKeyDict.Remove(key);
                    }
                }


                foreach (List<string> idList in CachedClassArticleKeyDict.Values)
                {
                    idList.Remove(articleId);
                }


                if (CachedTitleKeyDict.ContainsValue(articleId))
                {
                    List<string> removeKeyList = new List<string>();
                    lock (Locker)
                    {
                        foreach (string key in CachedTitleKeyDict.Keys)
                        {
                            if (CachedTitleKeyDict[key] == articleId)
                                removeKeyList.Add(key);
                        }
                    }
                    foreach (string key in removeKeyList)
                    {
                        CachedTitleKeyDict.Remove(key);
                    }

                }
                lock (Locker)
                {
                    if (CachedArticleDict.ContainsKey(articleId))
                    {
                        CachedArticleDict.Remove(articleId);
                    }
                }
            }

        }
        [Transaction(TransactionPropagation.Required)]
        private void CorrectArticleClass(Article article)
        {
            if (string.IsNullOrEmpty(article.big_class_id) && !string.IsNullOrEmpty(article.class_id))
            {
                IMainClassService mainClassService = ServiceFactory.GetMainClassService();
                article.big_class_id = mainClassService.GetBigClassBySmall(article.class_id).id;
                articleDao.UpdateArticleBigClass(article.id, article.big_class_id);
            }

        }

        private void NewCachedArticle(Article article)
        {
            if (article == null || string.IsNullOrEmpty(article.id) || CachedArticleDict.ContainsKey(article.id))
                return;

            lock (Locker)
            {
                CachedArticleDict.Add(article.id, article);

                if (string.IsNullOrEmpty(article.ref_id) && !CachedTitleKeyDict.ContainsKey(article.title))
                    CachedTitleKeyDict.Add(article.title, article.id);


                //referenced
                if (!string.IsNullOrEmpty(article.ref_id) && !CachedRefArticleKeyDict.ContainsKey(article.id))
                {
                    CachedRefArticleKeyDict.Add(article.id, article.ref_id);
                }



                //纠正补充small-big关系
                CorrectArticleClass(article);

                //small class
                CacheClassArticleKey(article.class_id, article.id);

                //big class
                CacheClassArticleKey(article.big_class_id, article.id);

                //module class
                CacheClassArticleKey(article.module_class_id, article.id);
            }
        }
        private void MaintainCachedArticleSite()
        {
            CachedArticleSiteKeyDict = this.GetArticleSiteDict();
            lock (Locker)
            {
                foreach (string articleId in CachedArticleSiteKeyDict.Keys)
                {
                    if (CachedArticleDict.ContainsKey(articleId))
                    {
                        CachedArticleDict[articleId].site_list = CachedArticleSiteKeyDict[articleId];
                    }

                }
            }
        }
        private void MaintainCachedArticleRef()
        {
            lock (Locker)
            {
                foreach (string refId in CachedRefArticleKeyDict.Keys)
                {
                    Article orginalArticle = this.GetArticle(CachedRefArticleKeyDict[refId]);
                    if (!orginalArticle.ref_by_list.Contains(refId))
                        orginalArticle.ref_by_list = orginalArticle.ref_by_list + " " + refId;
                }
            }

        }


        public Article GetRefArticle(string articleId)
        {
            if (!string.IsNullOrEmpty(articleId) && CachedRefArticleKeyDict.ContainsKey(articleId))
            {
                return GetArticle(CachedRefArticleKeyDict[articleId]);
            }
            else
                return null;
        }
        private void CacheClassArticleKey(string classId, string articleId)
        {
            if (string.IsNullOrEmpty(classId) || string.IsNullOrEmpty(articleId))
                return;


            List<string> classArticleList;
            if (CachedClassArticleKeyDict.ContainsKey(classId))
            {
                classArticleList = CachedClassArticleKeyDict[classId];
            }
            else
            {
                classArticleList = new List<string>();
                CachedClassArticleKeyDict.Add(classId, classArticleList);
            }

            if (!classArticleList.Contains(articleId))
            {
                classArticleList.Add(articleId);
            }



        }

        public Article GetArticle(string articleId, bool isWithContent)
        {
            if (string.IsNullOrEmpty(articleId))
                return new Article();

            articleId = articleId.Trim();

            if (IS_CACHE_CONTENT || !isWithContent)
            {
                if (!string.IsNullOrEmpty(articleId) && CachedArticleDict.ContainsKey(articleId))
                    return CachedArticleDict[articleId];

            }

            Article article = articleDao.GetArticle(articleId, isWithContent);
            if (!string.IsNullOrEmpty(article.id) && !CachedArticleDict.ContainsKey(articleId))
            {
                this.NewCachedArticle(article);
            }
            return article;
        }

        public Article GetArticle(string articleId)
        {
            return GetArticle(articleId, false);
        }

        public List<Article> GetArticleListByClassId(string classId)
        {

            List<Article> articleList = new List<Article>();
            if (!CachedClassArticleKeyDict.ContainsKey(classId))
                return articleList;


            foreach (string articleId in CachedClassArticleKeyDict[classId])
            {
                articleList.Add(GetArticle(articleId)); ;
            }

            return articleList;
        }

        public Dictionary<string, List<string>> GetArticleSiteDict()
        {
            return articleDao.GetArticleSiteDict();
        }

        public Article GetArticleByTitle(string articleTitle)
        {
            return GetArticleByTitle(articleTitle, false);
        }


        public int GetMaxSortSeqOfModule(string moduleClassId)
        {
            return articleDao.GetMinMaxSortSeqOfModule(moduleClassId, true);
        }

        public int GetMinSortSeqOfModule(string moduleClassId)
        {
            return articleDao.GetMinMaxSortSeqOfModule(moduleClassId, false);
        }

        public List<Article> GetTopArticleByModuleName(string moduleName, int count)
        {
            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            string moduleClassId = mainClassService.GetClassByName(moduleName, Total.SiteId).id;

            return GetTopArticleByModuleId(moduleClassId, count);


        }

        public List<Article> GetTopArticleByModuleId(string moduleId, int count)
        {
            List<Article> moduleArticleList = new List<Article>();
            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            lock (Locker)
            {
                foreach (Article article in CachedArticleDict.Values)
                {

                    if (article.module_class_id == moduleId)
                        moduleArticleList.Add(article);
                }
            }
            moduleArticleList.Sort(new ComparerArticle());
            return moduleArticleList.GetRange(0, moduleArticleList.Count >= count ? count : moduleArticleList.Count);


        }
        public List<Article> GetTopArticleByBigClassName(string bigClassName, int count)
        {
            IMainClassService mainClassService = ServiceFactory.GetMainClassService();
            string bigClassId = mainClassService.GetClassByName(bigClassName, Total.SiteId).id;
            return GetTopArticleByBigClassId(bigClassId, count);


        }
        public List<Article> GetTopArticleByBigClassId(string bigClassId, int count)
        {
            List<Article> bigArticleList = new List<Article>();
            lock (Locker)
            {
                foreach (Article article in CachedArticleDict.Values)
                {

                    if (article.big_class_id == bigClassId && article.site_list.Contains(Total.SiteId))
                        bigArticleList.Add(article);
                }
            }
            bigArticleList.Sort(new ComparerArticle());
            return bigArticleList.GetRange(0, bigArticleList.Count >= count ? count : bigArticleList.Count);
        }
        [Transaction(TransactionPropagation.Required)]
        public int InsertOrUpdateArticlePicture(ArticlePicture articlePicture)
        {
            if (articlePicture != null)
            {
                if (string.IsNullOrEmpty(articlePicture.id))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Total.PrefixArticlePictureId);
                    sb.Append(Total.SiteId);
                    int nextSeq = commonDao.GetNextSeq("article_picture");
                    sb.Append(nextSeq.ToString(Total.IdFormatString));

                    articlePicture.id = sb.ToString();
                    return articleDao.InsertArticlePicture(articlePicture);
                }
                else if (articleDao.GetArticlePicture(articlePicture.id) == null)
                {
                    return articleDao.InsertArticlePicture(articlePicture);
                }
                else
                {
                    return articleDao.UpdateArticlePicture(articlePicture);
                }
            }

            return 0;
        }

        [Transaction(TransactionPropagation.Required)]
        public bool InsertOrUpdateLawyerInfo(Lawyer lawyer)
        {
            if (lawyer == null)
                return false;
            if (string.IsNullOrEmpty(lawyer.id))
                lawyer.id = this.GenerateId();

            if (lawyer.sort_seq <= 0)
                lawyer.sort_seq = this.GetArticle(lawyer.article_id).sort_seq;

            if (articleDao.GetLawyerInfoByArticleId(lawyer.article_id) == null)
                return articleDao.InsertLawyerInfo(lawyer) > 0 ? true : false;
            else
                return articleDao.UpdateLawyerInfo(lawyer) > 0 ? true : false;

        }
        [Transaction(TransactionPropagation.Required)]
        public bool DeleteLawyerInfoByArticle(string articleId)
        {
            Lawyer lawyer = articleDao.GetLawyerInfoByArticleId(articleId);
            if (lawyer != null)
            {
                return articleDao.DeleteLawyerInfo(lawyer.id) > 0 ? true : false;
            }
            else
                return false;
        }

        [Transaction(TransactionPropagation.Required)]
        public bool InsertArticle(Article article)
        {

            if (article == null)
                return false;

            if (string.IsNullOrEmpty(article.id))
                article.id = this.GenerateId();

            if (article.sort_seq <= 0)
                article.sort_seq = articleDao.GetMaxArticleSortSeq() + 1;


            if (Total.EnableCGW)
            {
                article = ServiceFactory.GetCgwService().ReplaceArticle(article);
            }

            article.addtime = DateTime.Now;



            if (ExistsTitle(article.title, false))
            {

                string newTitle = article.title + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                Article deletedArticle = this.GetArticleByTitle(article.title);
                deletedArticle.title = newTitle;

                this.UpdateArticle(deletedArticle);
            }


            if (article.articlePicture != null)
                InsertOrUpdateArticlePicture(article.articlePicture);

            int success = articleDao.InsertArticle(article);
            if (success > 0)
            {


                foreach (string siteId in article.site_list)
                {
                    articleDao.InsertArticleSite(article.id, siteId);
                }

                this.NewCachedArticle(article);
                if (!string.IsNullOrEmpty(article.ref_id))
                    MaintainCachedArticleRef();


                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    foreach (string siteId in article.site_list)
                    {
                        backupDao.InsertBackupId(article.id, Total.PrefixArticleId, "I", siteId);
                    }

                }
            }
            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateArticle(Article article)
        {
            if (Total.EnableCGW)
                article = ServiceFactory.GetCgwService().ReplaceArticle(article);

            int success;

            if (article.articlePicture != null)
                InsertOrUpdateArticlePicture(article.articlePicture);


            if (string.IsNullOrEmpty(article.ref_id))
            {
                success = articleDao.UpdateArticle(article);
                if (success == 1)
                    articleDao.UpdateArticleSite(article.id, article.site_list);
            }
            else
            {
                success = articleDao.UpdateArticleRef(article);
                if (success == 1)
                    articleDao.UpdateArticleRefSite(article.id, article.site_list);
            }

            if (success == 1)
            {


                UpdateCachedArticle(article.id);

                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    foreach (string siteId in article.site_list)
                    {
                        backupDao.InsertBackupId(article.id, Total.PrefixArticleId, "U", siteId);
                    }
                }
            }
            return success > 0 ? true : false;
        }
        [Transaction(TransactionPropagation.Required)]
        public int UpdateAllStatic(bool isStatic, DateTime timestamp)
        {
            int success = articleDao.UpdateAllStatic(isStatic, timestamp);

            //更新cache
            foreach (string id in CachedArticleDict.Keys)
            {
                CachedArticleDict[id].is_static = isStatic;
                CachedArticleDict[id].last_mod = timestamp;
            }
            return success;

        }
        [Transaction(TransactionPropagation.Required)]
        private bool DeleteArticleRef(string articleId)
        {
            Article article = articleDao.GetArticle(articleId, false);

            foreach (string siteId in article.site_list)
            {
                articleDao.DeleteArticleRefSite(articleId, siteId);
            }

            bool success = articleDao.DeleteArticleRef(articleId);
            if (success)
            {
                Article origArticle = this.GetArticle(CachedRefArticleKeyDict[articleId]);
                origArticle.ref_by_list.Replace(articleId, "");
                CachedRefArticleKeyDict.Remove(articleId);

                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    foreach (string siteId in article.site_list)
                    {
                        backupDao.InsertBackupId(article.id, Total.PrefixArticleId, "D", siteId);
                    }
                }
            }

            return success;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool DeleteArticle(string articleId)
        {
            bool success = false;

            if (CachedRefArticleKeyDict.ContainsKey(articleId))
            {
                Article article = this.GetArticle(articleId);
                foreach (string siteId in article.site_list)
                {
                    articleDao.DeleteArticleRefSite(articleId, siteId);
                }
                success = this.DeleteArticleRef(articleId);

                return success;
            }
            else if (CachedArticleDict.ContainsKey(articleId))
            {
                Article article = this.GetArticle(articleId);
                article.title = article.title + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                foreach (string siteId in article.site_list)
                {
                    articleDao.DeleteArticleSite(articleId, siteId);
                }
                success = articleDao.DeleteArticle(article);

                if (success)
                {
                    CachedArticleDict.Remove(articleId);
                    if (!string.IsNullOrEmpty(article.ref_by_list))
                    {
                        string[] refArticleIdArray = article.ref_by_list.Split(' ');
                        foreach (string refArticleId in refArticleIdArray)
                        {
                            this.DeleteArticleRef(refArticleId);

                        }
                    }
                    //镜像同步
                    if (!Total.IsMirrorSite)
                    {
                        foreach (string siteId in article.site_list)
                        {
                            backupDao.InsertBackupId(article.id, Total.PrefixArticleId, "D", siteId);
                        }

                    }
                }
            }

            return success;
        }

        [Transaction(TransactionPropagation.Required)]
        public bool PurgeArticle(string articleId, string siteId)
        {
            bool success = false;

            Article article = this.GetArticle(articleId, false);
            if (article.articlePicture != null)
                articleDao.DeleteArticlePicture(article.articlePicture.id);

            if (articleDao.DeleteArticleSite(articleId, siteId) > 0)
            {
                CachedArticleSiteKeyDict[articleId].Remove(siteId);

                success = true;
            }
            if (articleDao.CountArticleSite(articleId) == 0)
            {
                articleDao.PurgeArticle(articleId);
                this.RemoveCachedArticle(articleId);
            }


            return success;
        }

        [Transaction(TransactionPropagation.Required)]
        public bool PurgeArticle(string articleId, IList<string> siteIdList)
        {
            bool success = true;

            Article article = this.GetArticle(articleId, false);
            if (article.articlePicture != null)
            {
                articleDao.DeleteArticlePicture(article.articlePicture.id);
                article.articlePicture = null;
            }

            if (siteIdList == null || siteIdList.Count == 0)
            {
                articleDao.PurgeArticle(articleId);
                this.RemoveCachedArticle(articleId);

                return success;
            }
            else
            {
                string[] siteIdArray = new string[siteIdList.Count];
                siteIdList.CopyTo(siteIdArray, 0);
                foreach (string siteId in siteIdArray)
                {
                    if (!this.PurgeArticle(articleId, siteId))
                        success = false;
                }
            }
            return success;
        }
        [Transaction(TransactionPropagation.Required)]
        public bool UpdateSortSeq(string articleId, int sortSeq)
        {
            int success = articleDao.UpdateSortSeq(articleId, sortSeq);
            if (success == 1)
                UpdateCachedArticle(articleId);

            return success > 0 ? true : false;
        }

        [Transaction(TransactionPropagation.Required)]
        public bool InsertArticleRef(Article articleRef)
        {
            if (articleRef == null || !CachedArticleDict.ContainsKey(articleRef.ref_id))
                return false;




            if (string.IsNullOrEmpty(articleRef.id))
                articleRef.id = this.GenerateId();

            if (articleRef.sort_seq <= 0)
                articleRef.sort_seq = articleDao.GetMaxArticleSortSeq() + 1;

            int success = articleDao.InsertArticleRef(articleRef);
            if (success > 0)
            {
                foreach (string siteId in articleRef.site_list)
                {
                    articleDao.InsertArticleRefSite(articleRef.id, siteId);
                }


                articleRef = articleDao.GetArticle(articleRef.id, IS_CACHE_CONTENT);
                this.NewCachedArticle(articleRef);
                MaintainCachedArticleRef();

                //镜像同步
                if (!Total.IsMirrorSite)
                {
                    foreach (string siteId in articleRef.site_list)
                    {
                        backupDao.InsertBackupId(articleRef.id, Total.PrefixArticleId, "I", siteId);
                    }

                }
            }

            return success > 0 ? true : false;
        }

        public List<Article> GetAllArticle(bool isWithContent)
        {
            List<Article> articleList = new List<Article>();
            if (IS_CACHE_CONTENT || !isWithContent)
            {
                articleList.AddRange(CachedArticleDict.Values);
            }
            else
            {
                articleList = articleDao.GetAllArticle(isWithContent);
            }
            return articleList;
        }


        public Dictionary<string, DateTime> GetAllArticleIdDict(bool isRefreshAll)
        {

            Dictionary<string, DateTime> articleIdTimeDic = new Dictionary<string, DateTime>();


            IMainClassService classService = ServiceFactory.GetMainClassService();
            string IntroModuleClassId = classService.GetClassByName(Total.ModuleNameLsjs, Total.SiteId).id;
            string FeeModuleClassId = classService.GetClassByName(Total.ModuleNameSfbz, Total.SiteId).id;


            foreach (string classId in CachedClassArticleKeyDict.Keys)
            {
                if (classId == IntroModuleClassId || classId == FeeModuleClassId)
                    continue;
                else
                {
                    Dictionary<string, DateTime> dict = GetArticleIdDictByClass(classId, isRefreshAll);
                    foreach (string id in dict.Keys)
                    {
                        if (!articleIdTimeDic.ContainsKey(id) && !String.IsNullOrEmpty(id))
                            articleIdTimeDic.Add(id, dict[id]);
                    }
                }

            }

            return articleIdTimeDic;
        }

        public Dictionary<string, DateTime> GetAllIntroIdDict(bool isRefreshAll)
        {
            IMainClassService classService = ServiceFactory.GetMainClassService();
            string IntroModuleClassId = classService.GetClassByName(Total.ModuleNameLsjs, Total.SiteId).id;

            return GetArticleIdDictByClass(IntroModuleClassId, isRefreshAll);
        }

        public Dictionary<string, DateTime> GetAllFeeIdDict(bool isRefreshAll)
        {
            IMainClassService classService = ServiceFactory.GetMainClassService();
            string FeeModuleClassId = classService.GetClassByName(Total.ModuleNameSfbz, Total.SiteId).id;
            return GetArticleIdDictByClass(FeeModuleClassId, isRefreshAll);
        }

        private Dictionary<string, DateTime> GetArticleIdDictByClass(string classId, bool isRefreshAll)
        {
            Dictionary<string, DateTime> articleIdTimeDic = new Dictionary<string, DateTime>();

            List<Article> articleList = GetArticleListByClassId(classId);
            foreach (Article article in articleList)
            {
                if (isRefreshAll)
                    articleIdTimeDic.Add(article.id, article.last_mod);
                else if (!article.is_static)
                    articleIdTimeDic.Add(article.id, article.last_mod);

            }

            return articleIdTimeDic;
        }

        public List<string> GetAllAuthorsList()
        {
            List<string> authorList = new List<string>();
            lock (Locker)
            {
                foreach (Article article in CachedArticleDict.Values)
                {
                    if (!string.IsNullOrEmpty(article.author) && !authorList.Contains(article.author))
                        authorList.Add(article.author);
                }
            }
            return authorList;
        }

        public List<ArticleBase> GetAllArticlesIndex()
        {
            return articleDao.GetAllArticlesIndex();
        }



        public Article GetDeletedArticle(string articleId, bool isWithContent)
        {
            Article article = articleDao.GetDeletedArticle(articleId, isWithContent);

            if (CachedArticleSiteKeyDict.ContainsKey(article.id))
            {
                article.site_list = CachedArticleSiteKeyDict[article.id];
            }

            return article;

        }


        public Article GetArticleByTitle(string articleTitle, bool isWithContent)
        {
            if (string.IsNullOrEmpty(articleTitle) || !CachedTitleKeyDict.ContainsKey(articleTitle))
                return new Article();
            else
            {
                return GetArticle(CachedTitleKeyDict[articleTitle], isWithContent);
            }
        }

        public Article GetArticleByModueSeq(string moduleClassId, int sortSeq)
        {
            List<Article> articleList = GetArticleListByClassId(moduleClassId);
            foreach (Article article in articleList)
            {
                if (article.sort_seq == sortSeq)
                    return article;
            }

            return new Article();

        }


        public void GetNeighborArticle(string articleId, out Article prevArticle, out Article nextArticle)
        {
            Article article = GetArticle(articleId);
            List<Article> neighborArticleList = GetTopArticle(article.big_class_id, article.class_id, article.module_class_id, 1, Int32.MaxValue);

            prevArticle = null;
            nextArticle = null;
            for (int i = 0; i < neighborArticleList.Count; i++)
            {
                if (neighborArticleList[i].id == articleId)
                {
                    if (i > 0)
                        nextArticle = neighborArticleList[i - 1];


                    if (i < neighborArticleList.Count - 1)
                        prevArticle = neighborArticleList[i + 1];


                    return;

                }
            }


        }


        public bool IsArticleRef(string articleId)
        {
            return CachedRefArticleKeyDict.ContainsKey(articleId);

        }
        [Transaction(TransactionPropagation.Required)]
        public int ClickArticle(string articleId)
        {
            articleDao.IncreaseClick(articleId);
            return GetArticle(articleId).click++;
        }


        public bool ExistsTitle(string articleTitle, bool isEnabled)
        {
            if (string.IsNullOrEmpty(articleTitle) || !CachedTitleKeyDict.ContainsKey(articleTitle))
                return false;
            else
            {
                Article article = this.GetArticleByTitle(articleTitle);
                return article.enabled == isEnabled;
            }

        }


        public int CountArticle(string moduleClassId, string bigClassId, string smallClassId)
        {
            return articleDao.CountArticle(moduleClassId, bigClassId, smallClassId);
        }
        [Transaction(TransactionPropagation.Required)]
        public string GenerateId()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Total.PrefixArticleId);
            sb.Append(Total.SiteId);


            int nextSeq = articleDao.GetMaxArticleIdSeq() + 1;
            sb.Append(nextSeq.ToString(Total.IdFormatString));
            return sb.ToString();
        }

        public List<Article> GetArticleList(string articleId)
        {
            List<Article> articleList = new List<Article>(1);
            articleList.Add(GetArticle(articleId));
            return articleList;
        }

        public List<Article> GetArticleList(string articleId, bool isWithContent)
        {
            List<Article> articleList = new List<Article>(1);
            articleList.Add(GetArticle(articleId, isWithContent));
            return articleList;
        }
        public List<Article> GetArticleListLikeTitle(string articleTitle)
        {
            List<Article> articleList = new List<Article>();
            foreach (Article article in GetAllArticle(false))
            {
                if (article.title.Contains(articleTitle))
                    articleList.Add(article);
            }

            return articleList;
        }

        public List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count, out int totalCount)
        {
            return GetTopArticle(bigClassId, smallClassId, moduleClassId, topBegin, count, out   totalCount, false);
        }
        public List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count, out int totalCount, bool ingoreNull)
        {
            List<Article> articleList = new List<Article>();
            List<string> titleList = new List<string>();
            lock (Locker)
            {
                foreach (Article article in CachedArticleDict.Values)
                {
                    if (article.enabled &&
                        (!ingoreNull && (string.IsNullOrEmpty(bigClassId) || string.IsNullOrEmpty(article.big_class_id) || article.big_class_id == bigClassId) && (string.IsNullOrEmpty(smallClassId) || string.IsNullOrEmpty(article.class_id) || article.class_id == smallClassId) && (string.IsNullOrEmpty(moduleClassId) || article.module_class_id == moduleClassId))
                        || (ingoreNull && (article.big_class_id == bigClassId || article.class_id == smallClassId || article.module_class_id == moduleClassId)))

                        if (!titleList.Contains(article.title))
                        {
                            titleList.Add(article.title);
                            articleList.Add(article);

                        }
                }
            }

            if (articleList.Count < topBegin)
            {
                totalCount = 0;
                return new List<Article>(0);
            }
            else
            {
                articleList.Sort(new ComparerArticle());

                if (articleList.Count < (topBegin - 1 + count))
                    count = articleList.Count - topBegin + 1;

                totalCount = articleList.Count;
                return articleList.GetRange(topBegin - 1, count);
            }
        }
        public List<Article> GetTopPictureArticle(string classId, int topBegin, int count)
        {

            List<Article> articleList = new List<Article>();
            List<string> titleList = new List<string>();
            lock (Locker)
            {
                foreach (Article article in CachedArticleDict.Values)
                {
                    if (article.enabled && article.articlePicture != null && article.articlePicture.id != null
                        && article.module_class_id == classId)

                        if (!titleList.Contains(article.title))
                        {
                            titleList.Add(article.title);
                            articleList.Add(article);

                        }
                }

            }
            if (articleList.Count < topBegin)
            {
                return new List<Article>(0);
            }
            else
            {
                articleList.Sort(new ComparerArticle());

                if (articleList.Count < (topBegin - 1 + count))
                    count = articleList.Count - topBegin + 1;

                return articleList.GetRange(topBegin - 1, count);
            }
        }
        public List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int topBegin, int count)
        {
            int totalCount;
            return GetTopArticle(bigClassId, smallClassId, moduleClassId, topBegin, count, out totalCount);
        }

        public List<Article> GetTopArticle(string bigClassId, string smallClassId, string moduleClassId, int count)
        {
            int totalCount;
            return GetTopArticle(bigClassId, smallClassId, moduleClassId, 1, count, out totalCount);
        }

        public SearchResult SearchArticle(string key, string searchType)
        {
            if (string.IsNullOrEmpty(key))
                return new SearchResult();
            else
                return articleDao.SearchArticle(key, searchType);
        }


        public List<Article> GetDeletedArticleList(bool isWithContent)
        {
            List<Article> deletedArticleList = articleDao.GetAllDeletedArticle(isWithContent);
            foreach (Article article in deletedArticleList)
            {
                if (CachedArticleSiteKeyDict.ContainsKey(article.id))
                {
                    article.site_list = CachedArticleSiteKeyDict[article.id];
                }
            }
            return deletedArticleList;
        }
        public List<Lawyer> GetAllLawyer()
        {
            List<Lawyer> lawyerList = articleDao.GetAllLawyer();
            foreach (Lawyer lawyer in lawyerList)
            {
                if (!string.IsNullOrEmpty(lawyer.article_id) && lawyer.introArticle == null)
                {
                    lawyer.introArticle = this.GetArticle(lawyer.article_id);
                }
            }


            return lawyerList;

        }

    }
}
